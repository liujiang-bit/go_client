Shader "custom/map/cm_territorty_line_lod"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _MainTex_ST.xy;
                return o;
            }

            

            fixed4 frag (v2f i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv);
                fixed2 uv_1;
					  uv_1 = i.uv;
					  if ((i.uv.y < 0.5)) {
					    discard;
					  } else {
					    uv_1.y = ((i.uv.y - 0.5) * 2.0);
					  };
					  fixed4 tmpvar_2;
					  tmpvar_2 = (tex2D (_MainTex, uv_1) * _Color);
                return tmpvar_2;
            }
            ENDCG
        }
    }
}
