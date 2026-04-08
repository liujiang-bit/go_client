Shader "custom/fog/cf_fog_border"
{
	Properties
	{
		[NoScaleOffset]_MainTex("Texture", 2D) = "white" {}
		_CloudTiling("cloud tiling", Vector) = (1,1,1,1)
		_Speed("wave speed", Vector) = (0,0,0,0)
		_FadeParameter("cloud fade at moutain edge", Range(0.0, 1.0)) = 0.15
		_DepthTweak("depth tweak", Float) = 1
	}
	SubShader
	{
		LOD 100
		Tags {
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"Queue" = "Transparent+115"
		}
		
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
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
				float4 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float4 color : COLOR;
				float4 screenPos : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			float4 _Speed;
			float4 _CloudTiling;

			v2f vert(appdata v)
			{
				v2f o;
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uv0.xy = ((worldPos.xz + (_Speed.xy * _Time.x)) * _CloudTiling.xy);
				o.uv0.zw = ((worldPos.xz + (_Speed.zw * _Time.x)) * _CloudTiling.zw);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv1 = v.uv;
				o.color = v.color;
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}

			sampler2D _MainTex;
			float4 _SpriteColor;
			float _FadeParameter;
			float _DepthTweak;
			sampler2D _CameraDepthTexture;

			fixed4 frag(v2f i) : SV_Target
			{
				float depth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, i.screenPos);
				float eyeDepth = LinearEyeDepth(depth) * _DepthTweak;

				float shade = clamp((_FadeParameter * (eyeDepth - i.screenPos.w)), 0.0, 1.0);

				float4 vColor = tex2D(_MainTex, i.uv0.xy);
				float4 hColor = tex2D(_MainTex, i.uv0.zw);

				float4 c = (vColor + hColor) / 2 * _SpriteColor;

				c.a = shade * i.color.a;
				return c;
			}
			ENDCG
		}
	}
}
