Shader "custom/landmap/tree-16-nomask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_TreeRot("tree rot", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "QUEUE" = "Transparent-16" "DisableBatching" = "True"}
		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
		ZWrite Off
        LOD 100
		//Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float4 center : COLOR0;
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

				fixed4 sprite_center = v.center * 180;
				float4 centerPos = (v.vertex- sprite_center) * float4(_TreeScale, _TreeScale, _TreeScale, 0);
				float s = sin(_TreeRot);
				float c = cos(_TreeRot);
				float2x2 matRot;
				matRot[0].x = c;
				matRot[0].y = s;
				matRot[1].x = -s;
				matRot[1].y = c;
				float4 newPos;
				newPos.xz = mul(matRot, centerPos.xz);
				newPos.yw = centerPos.yw;
				newPos = newPos + sprite_center;
				newPos.w = 1.0;
                o.vertex = UnityObjectToClipPos(newPos);
                o.uv = v.uv;
                return o;
            }

			float4 _SpriteColor;
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb = col.rgb*_SpriteColor.rgb;
                return col;
            }
            ENDCG
        }
    }
}
