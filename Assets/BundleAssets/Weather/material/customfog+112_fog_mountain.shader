Shader "custom/fog/+112_fog_mountain"
{
	Properties
	{
		[NoScaleOffset]_MainTex("Texture", 2D) = "white" {}
		[NoScaleOffset]_MaskTex("mask", 2D) = "white" { }
		_CloudTiling("cloud tiling", Vector) = (1,1,1,1)
		_Speed("wave speed", Vector) = (0,0,0,0)
		_FadeParameter("cloud fade at moutain edge", Range(0.0, 1.0)) = 0.15
		_DepthTweak("depth tweak", Range(0.0, 2.0)) = 1
		_Width("width of fog girdle", Range(1, 2)) = 1
		_Height("height of fog girdle", Range(8, 20)) = 8
		_Intensity("intensity of fog girdle", Range(0.5, 2)) = 1
		_ShadowColor("shadow color", Color) = (0,0,0,0)
	}
		SubShader
		{
			Tags {
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"Queue" = "Transparent+112"
			}

			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha
				ZClip Off
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
				float _Height;

				v2f vert(appdata v)
				{
					v2f o;

					float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
					worldPos.y = _Height;
					float4 objPos = mul(unity_WorldToObject, worldPos);
					o.vertex = UnityObjectToClipPos(objPos);
					o.uv0.xy = ((worldPos.xz + (_Speed.xy * _Time.x)) * _CloudTiling.xy);
					o.uv0.zw = ((worldPos.xz + (_Speed.zw * _Time.x)) * _CloudTiling.zw);

					o.uv1 = v.uv;
					o.screenPos = ComputeScreenPos(o.vertex);
					return o;
				}

				sampler2D _MainTex;
				sampler2D _MaskTex;
				float4 _SpriteColor;
				float _FadeParameter;
				float _DepthTweak;
				sampler2D _CameraDepthTexture;
				float _Width;
				float _Intensity;

				float4 _ShadowColor;
				fixed4 frag(v2f i) : SV_Target
				{
					float depth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, i.screenPos);
					float eyeDepth = LinearEyeDepth(depth) * _DepthTweak;

					float shade = _FadeParameter * (eyeDepth - i.screenPos.w);
					shade = clamp(((0.5 - abs((0.5 - (shade / _Width)))) * _Intensity) , 0, 1);

					float4 mask = tex2D(_MaskTex, i.uv1);

					float4 vColor = tex2D(_MainTex, i.uv0.xy);
					float4 hColor = tex2D(_MainTex, i.uv0.zw);
					float4 mAColor = clamp(mask.a*2 - 1, 0.0, 1.0);
					float4 c = ((vColor + hColor) / 2 * _SpriteColor) *(mask + mAColor + _ShadowColor);

					c.a = shade* _ShadowColor.a;
					return c;
				}
				ENDCG
			}
		}
}
