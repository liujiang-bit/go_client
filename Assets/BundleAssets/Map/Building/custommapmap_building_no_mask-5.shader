Shader "custom/map/map_building_no_mask-5"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent-5" "RenderType" = "Transparent" "CanUseSpriteAtlas" = "True"}
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            sampler2D _MainTex;
            fixed4 _SpriteColor;
            float _CityTransparency;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv)*_SpriteColor;
                col.w = col.w * _CityTransparency;
                return col;
            }
            ENDCG
        }
    }
}