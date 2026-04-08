Shader "custom/map/cm_slg_plane_lod"
{
	Properties
	{
		_Color0("Color0", Vector) = (1,1,1,1)
		_Color1("Color1", Vector) = (1,1,1,1)
		_Color2("Color2", Vector) = (1,1,1,1)
		_Color3("Color3", Vector) = (1,1,1,1)
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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			uniform float4 _Color0;
			uniform float4 _Color1;
			uniform float4 _Color2;
			uniform float4 _Color3;
			uniform sampler2D _Splat;
			uniform float4 _Splat_ST;
			uniform float4 _SpriteColor;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _Splat);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 splat = tex2D(_Splat, i.uv);
				fixed4 col = (splat.x * _Color0 + splat.y * _Color1 + +splat.z * _Color2 + ((1 - splat.x - splat.y - splat.z)*_Color3))*_SpriteColor;
				return col;
			}
			ENDCG
		}
	}
}
