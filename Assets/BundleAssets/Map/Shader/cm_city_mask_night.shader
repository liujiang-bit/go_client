Shader "custom/map/cm_city_mask_night"
{
    Properties
    {
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "black" {}
		[PerRendererData] _Mask("mask texture", 2D) = "white" {}
		_Color_1("Color 1", Color) = (1,1,1,1)
		_Color_2("Color 2", Color) = (1,1,1,1)
		_Color_3("Color Light", Color) = (1,1,1,1)
    }
    SubShader
	{
		Tags
		{
			"Queue" = "Transparent-5"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex SpriteVert
			#pragma fragment SpriteFrag
			#pragma target 2.0

			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR0;
				fixed3 lightColor : COLOR1;
			};

			sampler2D _MainTex;
			sampler2D _Mask;
			fixed4 _Color_1;
			fixed4 _Color_2;
			fixed4 _Color_3;
			fixed4 _SpriteColor;
			float _CityTransparency;
			v2f SpriteVert(appdata_t IN)
			{
				v2f OUT;

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				float l = clamp((abs(sin((_Time.y * 0.5))) + 0.5), 0.0, 1.0);
				OUT.lightColor = _Color_3.rgb * l;
				return OUT;
			}

			fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);
				return color;
			}

			fixed4 SpriteFrag(v2f IN) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, IN.texcoord)*IN.color;
				fixed4 mask = tex2D(_Mask, IN.texcoord);

				fixed mask_1 = (mask.r * 2.0);
				fixed tmpvar_5;
				tmpvar_5 = clamp((mask_1 - 1.0), 0.0, 1.0);
				//先去掉阵营
				//tmpvar_5 = 0.0f;

				fixed4 c_2;
				c_2.xyz = (((col.rgb *((1.0 - tmpvar_5) - mask.g)) + ((2.0 * col.rgb)*(_Color_1.rgb * tmpvar_5))) + ((2.0 * col.rgb) * (_Color_2.rgb * mask.g)));
				c_2 = (c_2 * _SpriteColor);
				c_2.w = (col.a * clamp(mask_1, 0.0, 1.0))*_CityTransparency;
				c_2.rgb = c_2.rgb + IN.lightColor * mask.b;
				return c_2;
			}
			ENDCG
		}
	}
}