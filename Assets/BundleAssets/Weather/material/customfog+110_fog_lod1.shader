Shader "custom/fog/+110_fog_lod1"
{
	Properties
	{
		[NoScaleOffset]_MainTex("Texture", 2D) = "white" {}
		[NoScaleOffset]_MaskTex("mask", 2D) = "white" { }
		[NoScaleOffset]_MaskTexAlpha("maskAlpha", 2D) = "white" { }
		_CloudTiling("cloud tiling", Vector) = (1,1,1,1)
		_Speed("wave speed", Vector) = (0,0,0,0)
		_FadeParameter("cloud fade at moutain edge", Range(0.0, 1.0)) = 0.15
		_ShadowFadeParameter("shadow fade at moutain edge", Range(0.0, 1.0)) = 0.5
		_ShadowColor("shadow color", Color) = (0,0,0,0)
		_DepthTweak("depth tweak", Float) = 1
	}
	SubShader
	{
		LOD 100
		Tags {
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"Queue" = "Transparent+110"
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
				float2 uv : TEXCOORD0;
			};
			struct v2f
			{
				float2 uv0 : TEXCOORD0;
				float4 screenPos : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(float3(v.vertex.x, v.vertex.y - 5, v.vertex.z));
				float4 tmpvar_2 = mul(unity_ObjectToWorld, v.vertex);
				o.uv0 = v.uv;
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}

			sampler2D _MaskTexAlpha;
			sampler2D _CameraDepthTexture;
			fixed4 _ShadowColor;
			float _ShadowFadeParameter;
			float _FogShade;

			fixed4 frag(v2f i) : SV_Target
			{
				float depth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, i.screenPos);
				float eyeDepth = LinearEyeDepth(depth);
				float shade = clamp((_ShadowFadeParameter * (eyeDepth - i.screenPos.w)), 0.0, 1.0);
				float4 c = _ShadowColor;
				c.a = c.a * tex2D(_MaskTexAlpha, i.uv0).w * shade*_FogShade;
				return c;
			}
			ENDCG
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
				float2 uv : TEXCOORD0;
			};
			struct v2f
			{
				float4 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
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
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}

			sampler2D _MainTex;
			sampler2D _MaskTex;
			sampler2D _MaskTexAlpha;
			float4 _SpriteColor;
			float _FadeParameter;
			float _DepthTweak;
			float _ShadowFadeParameter;
			sampler2D _CameraDepthTexture;
			float _FogShade;

			fixed4 frag(v2f i) : SV_Target
			{
				float depth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, i.screenPos);
				float eyeDepth = LinearEyeDepth(depth) * _DepthTweak;

				float shade = clamp((_FadeParameter * (eyeDepth - i.screenPos.w)), 0.0, 1.0);

				float4 mask = tex2D(_MaskTex, i.uv1);
				float4 maskAlpha = tex2D(_MaskTexAlpha, i.uv1);
				maskAlpha.a = maskAlpha.a * 2;

				float4 vColor = tex2D(_MainTex, i.uv0.xy);
				float4 hColor = tex2D(_MainTex, i.uv0.zw);

				float4 mColor = 1 - float4(5.36, 0.8797959, 0.0, 0.0) * mask.a;
				float4 mAColor = clamp(maskAlpha.a-1, 0.0, 1.0);
				float4 c = (vColor + hColor) / 2 * _SpriteColor * (mColor+ mAColor);

				c.a = clamp(maskAlpha.a, 0, 1) * shade*_FogShade;
				return c;
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 80
		Tags {
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"Queue" = "Transparent+110"
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
				float2 uv : TEXCOORD0;
			};
			struct v2f
			{
				float4 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
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
				return o;
			}

			sampler2D _MainTex;
			sampler2D _MaskTex;
			sampler2D _MaskTexAlpha;
			float4 _SpriteColor;
			float _FogShade;

			fixed4 frag(v2f i) : SV_Target
			{
				float4 mask = tex2D(_MaskTex, i.uv1);
				float4 maskAlpha = tex2D(_MaskTexAlpha, i.uv1);
				maskAlpha.a = maskAlpha.a * 2;

				float4 vColor = tex2D(_MainTex, i.uv0.xy);
				float4 hColor = tex2D(_MainTex, i.uv0.zw);

				float4 mColor = 1 - float4(5.36, 0.8797959, 0.0, 0.0) * mask.a;
				float4 mAColor = clamp(maskAlpha.a - 1, 0.0, 1.0);
				float4 c = (vColor + hColor) / 2 * _SpriteColor * (mColor + mAColor);

				c.a = clamp(maskAlpha.a, 0, 1)*_FogShade;
				return c;
			}
			ENDCG
		}
	}
}
