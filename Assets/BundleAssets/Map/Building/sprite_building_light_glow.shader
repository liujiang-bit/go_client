Shader "custom/map/sprite_building_light_glow"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_Color("light color", Color) = (1,1,1,1)
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
		Blend SrcAlpha One, SrcAlpha One

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
				//fixed4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				//fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			fixed4 _Color;
			v2f SpriteVert(appdata_t IN)
			{
				v2f OUT;

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				//float a = clamp((abs(sin((_Time.y * 0.5))) + 0.5), 0.0, 1.0);
				//OUT.color = _Color*a;
				return OUT;
			}
			fixed4 SpriteFrag(v2f IN) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, IN.texcoord)*_Color;
				return col;
			}
			ENDCG
		}
	}
}