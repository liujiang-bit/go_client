Shader "custom/landmap/z_cm_slg_mountain_edge"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"RenderType"="Transparent" "Queue"="Transparent-60"}

        Pass{
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
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
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f{
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float alpha : TEXCOORD1;
            };
            
            v2f vert(appdata v)
			{
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.alpha = v.color.a; 
                
                return o;
            }
			float4 _SpriteColor;
            
            float4 frag(v2f i) : SV_Target
			{
                float4 color = tex2D(_MainTex, i.uv);
				color.rgb *= _SpriteColor;
                color.a = i.alpha;
                
                return color;
            }
            
            
            ENDCG
        
        }
    }
    FallBack "Diffuse"
}
