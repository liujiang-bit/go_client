Shader "custom/map/city_building_ani_nomask-5"
{
    Properties
    {
		_MainTex("Sprite Texture", 2D) = "white" {}
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
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			fixed4 _SpriteColor;
			float _CityTransparency;
			v2f SpriteVert(appdata_t IN)
			{
				v2f OUT;

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				return OUT;
			}
			fixed4 SpriteFrag(v2f IN) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, IN.texcoord);
				col.a = (col.a * 2.0);
				col.rgb = col.rgb * (lerp(1, IN.color*2, clamp(col.a-1, 0, 1)) * _SpriteColor).rgb;
				//col.rgb = col.rgb * (mix(float4(1.0, 1.0, 1.0, 1.0), (IN.color * 2.0), clamp(col.a - 1.0, 0.0, 1.0)) * _SpriteColor).rgb;
				col.a = (col.a * _CityTransparency);
				return col;
			}
			ENDCG
		}
	}
}