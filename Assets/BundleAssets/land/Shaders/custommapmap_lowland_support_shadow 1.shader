
Shader "custom/landmap/lowland_support_shadow"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "IgnoreProjector"="True" "RenderType"="Opaque" "Queue"="Geometry+5" }

        Pass{

            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float4 _LightColor0;    // 全局光照值
            

            struct appdata{
                float4 vertex : POSITION;
                float4 color : COLOR;
                float3 normal : NORMAL; 
                float2 uv : TEXCOORD0;
            };
            
            struct v2f{
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float3 worldNormal = normalize(mul(unity_WorldToObject, v.normal));
                float dotValue = max(0, dot(worldNormal, _WorldSpaceLightPos0));
                o.color = (_LightColor0 * dotValue);
                
                
                return o;
            }
            
            float4 frag(v2f i) : SV_Target 
            {
            
                float4 color;
                float4 ambient = UNITY_LIGHTMODEL_AMBIENT;
                color = tex2D(_MainTex, i.uv) * (ambient * 2 + i.color);
            
                return color;
            }
            
            
            ENDCG
        
        }
    }
    FallBack "Diffuse"
}
