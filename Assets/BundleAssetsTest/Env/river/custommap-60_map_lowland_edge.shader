Shader "Custom/custommap_60_map_lowland_edge"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "IgnoreProjector"="True" "RenderType"="Transparent" "Queue"="Transparent-60" }

        Pass{
        
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _LightColor0;
            
            struct appdata{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f{
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };
            
            v2f vert(appdata v){
                v2f o;
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float3 worldNor = normalize(mul(UNITY_MATRIX_M, v.normal));
                float4 color;
                color = (UNITY_LIGHTMODEL_AMBIENT * 2) + (_LightColor0 * max(0, dot(worldNor, _WorldSpaceLightPos0.xyz)));
                color.w = v.color.w;
                
                o.color = color;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                return o;
            }
            
            float4 frag(v2f i) : SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv) * i.color;
                color.w = i.color.w;
                return color;
            }
            
            
            ENDCG
        
        }
    }
    FallBack "Diffuse"
}
