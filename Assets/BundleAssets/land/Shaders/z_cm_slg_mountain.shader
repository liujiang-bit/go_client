Shader "custom/landmap/z_cm_slg_mountain"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}

        Pass{
        
            
            CGPROGRAM
            
			#pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f{
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            v2f vert(appdata v)
			{
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                return o;
            }
            
			float4 _SpriteColor;
            float4 frag(v2f i) : SV_Target
			{
                float4 color = tex2D(_MainTex, i.uv); 
				color.rgb *= _SpriteColor;
                return color;
            }
            
            
            ENDCG
        
        }
    }
    FallBack "Diffuse"
}
