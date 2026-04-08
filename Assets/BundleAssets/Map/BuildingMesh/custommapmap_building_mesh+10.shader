Shader "custom/map/map_building_mesh+10"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "QUEUE" = "Transparent+10" "RenderType" = "Transparent"  }
		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		ZTest Always
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

				float3x3 tmpvar_3;
				tmpvar_3[0] = unity_ObjectToWorld[0].xyz;
				tmpvar_3[1] = unity_ObjectToWorld[1].xyz;
				tmpvar_3[2] = unity_ObjectToWorld[2].xyz;

				float3 tmpvar_4;
				tmpvar_4.y = 0.0;
				tmpvar_4.xz = v.vertex.xz;

                o.uv = (mul(tmpvar_3, tmpvar_4)).xz * _MainTex_ST.xy;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
