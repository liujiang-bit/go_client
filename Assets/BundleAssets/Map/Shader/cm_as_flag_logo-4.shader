Shader "custom/map/cm_as_flag_logo_4"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent-4" "RenderType" = "Transparent" }
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
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
            };
            
            fixed4 _SpriteColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color*_SpriteColor;
                return o;
            }
            
            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return col;
            }
            ENDCG
        }
    }
}