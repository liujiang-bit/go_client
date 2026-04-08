Shader "custom/sprite/unit_fade_out-2"
{
    Properties
    {
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "black" {}
    }
    SubShader
	{
		Tags
		{
			"Queue" = "Transparent-2"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Cull Off
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

			v2f SpriteVert(appdata_t IN)
			{
				v2f OUT;

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				return OUT;
			}

			sampler2D _MainTex;
			fixed4 _SpriteColor;
			fixed4 SpriteFrag(v2f IN) : SV_Target
			{
			  fixed4 col = tex2D(_MainTex, IN.texcoord);
			  col.a = (col.a * 2.0);
			  col.rgb = col.rgb * (lerp(1, IN.color * 2, col.a - 1.0) * _SpriteColor).rgb;
			  col.rgb = clamp(col.a, 0.0, 1.0)*col.rgb;
			  col.w = (col.w * IN.color.w);
			  return col;
			}
			ENDCG
		}
	}
}