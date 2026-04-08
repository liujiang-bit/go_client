Shader "custom/map/map_cloud+200"
{
    Properties
    {
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
    }
    SubShader
	{
		Tags
		{
			"Queue" = "Transparent+200"
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
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			fixed4 _SpriteColor;
			v2f SpriteVert(appdata_t IN)
			{
				v2f OUT;

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color*_SpriteColor;
				return OUT;
			}

			sampler2D _MainTex;

			fixed4 SpriteFrag(v2f IN) : SV_Target
			{
				fixed4 color = tex2D(_MainTex, IN.texcoord)*IN.color;
				return color;
			}
			ENDCG
		}
	}
}