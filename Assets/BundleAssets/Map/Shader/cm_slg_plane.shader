Shader "custom/map/cm_slg_plane"
{
	Properties
	{
		_Tex0("Texture0(R)", 2D) = "white" {}
		_Tex1("Texture1(G)", 2D) = "white" {}
		_Tex2("Texture2(B)", 2D) = "white" {}
		_Tex3("Texture3(1-R-G-B)", 2D) = "white" {}
		_Splat("Mask", 2D) = "gray" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" "QUEUE" = "Geometry-5"}
		LOD 100
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
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float2 uv3 : TEXCOORD3;
				float2 uv4 : TEXCOORD4;
				float4 vertex : SV_POSITION;
			};

			uniform sampler2D _Tex0;
			uniform float4 _Tex0_ST;
			uniform sampler2D _Tex1;
			uniform float4 _Tex1_ST;
			uniform sampler2D _Tex2;
			uniform float4 _Tex2_ST;
			uniform sampler2D _Tex3;
			uniform float4 _Tex3_ST;
			uniform sampler2D _Splat;
			uniform float4 _Splat_ST;
			uniform float4 _SpriteColor;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv0 = TRANSFORM_TEX(v.uv, _Tex0);
				o.uv1 = TRANSFORM_TEX(v.uv, _Tex1);
				o.uv2 = TRANSFORM_TEX(v.uv, _Tex2);
				o.uv3 = TRANSFORM_TEX(v.uv, _Tex3);
				o.uv4 = TRANSFORM_TEX(v.uv, _Splat);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 splat = tex2D(_Splat, i.uv4);
				fixed4 col = (splat.x * tex2D(_Tex0, i.uv0) + splat.y * tex2D(_Tex1, i.uv1) + +splat.z * tex2D(_Tex2, i.uv2) + ((1 - splat.x - splat.y - splat.z)*tex2D(_Tex3, i.uv3)))*_SpriteColor;
				return col;
			}
			ENDCG
		}
	}
}
