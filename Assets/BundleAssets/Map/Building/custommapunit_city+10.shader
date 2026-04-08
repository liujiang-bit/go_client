Shader "custom/map/unit_city+10"
{
    Properties
    {
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "black" {}
    }
    SubShader
	{
		Tags
		{
			"Queue" = "Geometry+10"
			"IgnoreProjector" = "True"
			"RenderType" = "Opaque"
		}

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
			
			  //fixed4 tmpvar_2 = tex2D (_MainTex, IN.texcoord);					  
			  //fixed4  c_1;
			  //c_1.xyz = tmpvar_2.xyz;
			  //c_1.w = (tmpvar_2.w * 2.0);

			  //c_1.xyz = tmpvar_2.xyz * (mix(1, IN.color * 2.0, float4(c_1.w - 1.0)) * _SpriteColor).xyz;

			  //clip(c_1.w - 0.7);

			  //return c_1;

			  fixed4 col = tex2D(_MainTex, IN.texcoord);
			  col.a = (col.a * 2.0);
			  col.rgb = col.rgb * (lerp(1, IN.color * 2, col.a - 1.0) * _SpriteColor).rgb;
			  clip(col.a - 0.1);
			  return col;
			}
			ENDCG
		}
	}
}