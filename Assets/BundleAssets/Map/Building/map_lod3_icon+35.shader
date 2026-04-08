Shader "custom/sprite/map_lod3_icon+35"
{
    Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _Color ("Main Color", Color) = (0.1,0.7,0.5,1)
  }
  SubShader
  {
    Tags{ "Queue" = "Transparent+47" "IgnoreProjector" = "True" "RenderType" = "Transparent" "CanUseSpriteAtlas" = "True" }

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass
    {
        CGPROGRAM
      			#pragma vertex vert
			#pragma fragment frag
			
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
			fixed4 _Color;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				return col;
			}
      ENDCG
    }
  }
}

//{
//    Properties
//    {
//        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
//        _Color ("Main Color", Color) = (0.1,0.7,0.5,1)
//    }
//    SubShader
//    {
//        Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent+35" "RenderType" = "Transparent" "CanUseSpriteAtlas" = "True" }
//		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
//		ZWrite Off
//		Pass {
//		    CGPROGRAM
//			#pragma vertex vert
//			#pragma fragment frag
//			
//			struct appdata
//			{
//				float4 vertex : POSITION;
//				float2 uv : TEXCOORD0;
//			};
//
//			struct v2f
//			{
//				float2 uv : TEXCOORD0;
//				float4 vertex : SV_POSITION;
//			};
//
//			sampler2D _MainTex;
//			fixed4 _Color;
//
//			v2f vert (appdata v)
//			{
//				v2f o;
//				o.vertex = UnityObjectToClipPos(v.vertex);
//				o.uv = v.uv;
//				return o;
//			}
//			
//			fixed4 frag (v2f i) : SV_Target
//			{
//				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
//				return col;
//			}
//			ENDCG
//			
//		}
//    }
//}
