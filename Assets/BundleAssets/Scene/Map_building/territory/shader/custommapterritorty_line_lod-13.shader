Shader "custom/map/territorty_line_lod-13"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Vector) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent-13" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
		ZWrite Off

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
                fixed4 color : COLOR; //颜色
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR; //颜色
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv =  v.uv * _MainTex_ST.xy;
                o.color = v.color;
                return o;
            }

            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 uv_1;
				uv_1.x = i.uv.x;
				if ((i.uv.y < 0.5)) {
					  discard;
				} else {
					  uv_1.y = ((i.uv.y - 0.5) * 2.0);
				};
				fixed4 col = tex2D (_MainTex, i.uv) * _Color*i.color;
			    return col;
            }
            ENDCG
        }
    }
}
