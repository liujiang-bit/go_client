Shader "custom/map/sprite_building_move"
{
    Properties
    {
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "black" {}
		[PerRendererData] _Mask("mask texture", 2D) = "white" {}
		_Bouncing("Bounce Range", Float) = 0
		_ClickBounceStep("Click Bounce Step", Float) = 0
		_Scale("Scale", Float) = 1
		_Color_1("Color 1", Color) = (1,1,1,1)
		_Color_2("Color 2", Color) = (1,1,1,1)
    }
    SubShader
	{
		Tags
		{
			"Queue" = "Transparent+20"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest Always
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
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			float _Scale;
			float _Bouncing;
			float _ClickBounceStep;

			v2f SpriteVert(appdata_t IN)
			{
				v2f OUT;
				float4x4 tmpvar_3;
				tmpvar_3[0].x = _Scale;
				tmpvar_3[0].y = 0.0;
				tmpvar_3[0].z = 0.0;
				tmpvar_3[0].w = 0.0;
				tmpvar_3[1].x = 0.0;
				tmpvar_3[1].y = _Scale;
				tmpvar_3[1].z = 0.0;
				tmpvar_3[1].w = 0.0;
				tmpvar_3[2].x = 0.0;
				tmpvar_3[2].y = 0.0;
				tmpvar_3[2].z = _Scale;
				tmpvar_3[2].w = 0.0;
				tmpvar_3[3].x = 0.0;
				tmpvar_3[3].y = 0.0;
				tmpvar_3[3].z = 0.0;
				tmpvar_3[3].w = 1.0;

				float4 tmpvar_5;
				tmpvar_5.w = 1.0f;
				tmpvar_5.xyz = mul(tmpvar_3, IN.vertex).xyz;
				OUT.vertex = UnityObjectToClipPos(tmpvar_5);
				OUT.vertex.y = (OUT.vertex.y + ((((sin((_Time.y * 5.0)) / 20.0) + 0.05)* _Bouncing) + _ClickBounceStep));

				float tmpvar_6 = abs(sin((_Time.y * 2.25)));
				OUT.color.x = ((tmpvar_6 / 4.0) + 0.75);
				OUT.color.yzw = 0;
				OUT.texcoord = IN.texcoord;
				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _Mask;
			fixed4 _Color_1;
			fixed4 _Color_2;
			fixed4 _SpriteColor;
			float _CityTransparency;

			fixed4 SpriteFrag(v2f IN) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, IN.texcoord);
				fixed4 mask = tex2D(_Mask, IN.texcoord);

				fixed mask_1 = (mask.r * 2.0);
				fixed tmpvar_5;
				tmpvar_5 = clamp((mask_1 - 1.0), 0.0, 1.0);
				//先去掉阵营
			//	tmpvar_5 = 0.0f;

				fixed4 c_2;
				c_2.xyz = (((col.rgb *((1.0 - tmpvar_5) - mask.g)) + ((2.0 * col.rgb)*(_Color_1.rgb * tmpvar_5))) + ((2.0 * col.rgb) * (_Color_2.rgb * mask.g)));
				c_2 = (c_2 * IN.color.r * _SpriteColor);
				c_2.a = mask_1;
				return c_2;
			}
			ENDCG
		}
	}
}