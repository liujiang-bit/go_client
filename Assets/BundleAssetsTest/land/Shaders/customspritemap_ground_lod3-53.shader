Shader "custom/land/sprite/ground_lod3-53"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("chagne color channel 1", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "QUEUE" = "Transparent-53" }
		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off
        LOD 100

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			float4 _SpriteColor;
			float4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb = col.rgb*_SpriteColor.rgb*_Color;
                return col;
            }
            ENDCG
        }
    }
}
