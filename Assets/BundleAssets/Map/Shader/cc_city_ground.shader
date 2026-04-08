Shader "custom/city/cc_city_ground" 
{
	Properties {
		_Color("BaseColor", Color) = (1,1,1,1)
		_Tex0("Texture0", 2D) = "white" {}
		_Tex1("Texture1", 2D) = "white" {}
		_Tex2("Texture1", 2D) = "white" {}
		[NoScaleOffset]_Splat("Mask", 2D) = "gray" {}
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Geometry+9" "RenderType" = "Opaque" }
		ColorMask RGB
		ZWrite Off
		Stencil {
			Ref 10
			Comp Always
			Pass Replace
			Fail Keep
			ZFail Keep
		}
		Pass {
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
                float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float2 uv : TEXCOORD3;
                float4 vertex : SV_POSITION;
            };

            sampler2D _Tex0;
            float4 _Tex0_ST;
			sampler2D _Tex1;
			float4 _Tex1_ST;
			sampler2D _Tex2;
			float4 _Tex2_ST;
			sampler2D _Splat;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv0 = TRANSFORM_TEX(v.uv, _Tex0);
				o.uv1 = TRANSFORM_TEX(v.uv, _Tex1);
				o.uv2 = TRANSFORM_TEX(v.uv, _Tex2);
				o.uv = v.uv;
                return o;
            }

			float4 _SpriteColor;
			float4 _Color;
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_Splat, i.uv);				
				col = (((col.x * tex2D(_Tex0, i.uv0)) + (col.y * tex2D(_Tex1, i.uv1)) + (col.z * tex2D(_Tex2, i.uv2))) * _Color * _SpriteColor);
                return col;
            }
            ENDCG
		}
	}
}