Shader "custom/map_lod/+30_map_sprite_lod1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Mask("mask texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "QUEUE" = "Transparent+30" }
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
			sampler2D _Mask;

			fixed _TreeScale;
			float _TreeRot;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			float4 _SpriteColor;
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				col.a = tex2D(_Mask, i.uv).x;
				col.rgb = col.rgb*_SpriteColor.rgb;
                return col;
            }
            ENDCG
        }
    }
}
