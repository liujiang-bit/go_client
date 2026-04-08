Shader "custom/map/map_building_mesh+10" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent+10" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
		ZWrite Off

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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				float3 pos = float3(v.vertex.x,0,v.vertex.z);
				float3 wolrdPos =  mul((float3x3)unity_ObjectToWorld, pos).xyz;
				o.uv = wolrdPos.xz * _MainTex_ST.xy;
				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) ;
				return col;
			}
			ENDCG
			
		}
	}
}