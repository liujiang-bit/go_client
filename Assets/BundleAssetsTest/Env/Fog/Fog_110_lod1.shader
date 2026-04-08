Shader "Unlit/Fog110"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_MaskTex("mask", 2D) = "white" { }
		_MaskTexAlpha("maskAlpha", 2D) = "white" { }
		_CloudTiling("cloud tiling", Vector) = (1,1,1,1)
		_Speed("wave speed", Vector) = (0,0,0,0)
		_FadeParameter("cloud fade at moutain edge", Float) = 0.15
		_ShadowFadeParameter("shadow fade at moutain edge", Float) = 0.5
		_ShadowColor("shadow color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags {
			"IgnoreProjector" = "True"
			"RenderType"="Transparent"
			"Queue" = "Transparent+110" 
		}

        Pass
        {
			LOD 100
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
				float4 uv2 : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _MaskTex;
			sampler2D _MaskTexAlpha;
			float4 _Speed;
			float4 _CloudTiling;
			fixed4 _ShadowColor;
			float _ShadowFadeParameter;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex.xyz);
				float3 tmpvar_2 = UnityObjectToClipPos(v.vertex).xyz;

				o.uv0.xy = ((tmpvar_2.xy + (_Speed.xy * _Time.x)) * _CloudTiling.xy);
				o.uv0.zw = ((tmpvar_2.xy + (_Speed.zw * _Time.x)) * _CloudTiling.zw);

				o.uv1 = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = ComputeScreenPos(o.vertex);
                return o;
            }

			uniform sampler2D _CameraDepthTexture;
            fixed4 frag (v2f i) : SV_Target
            {
				float4 finalColor;
				float4 u_xlat10_0;
				float4 u_xlat10_1;
				float4 u_xlat16_0;
				float4 u_xlat16_1;
				float4 u_xlat16_2;
				float4 u_xlat16_5;
				float4 u_xlat0;
				float4 u_xlat10_9;
				u_xlat10_0.xyz = tex2D(_MainTex, i.uv0.xy).xyz;
				u_xlat10_1.xyz = tex2D(_MainTex, i.uv0.zw).xyz;
				u_xlat16_0.xyz = u_xlat10_0.xyz + u_xlat10_1.xyz;
				u_xlat0.xyz = u_xlat16_0.xyz * _ShadowColor.xyz;
				u_xlat0.xyz = u_xlat0.xyz * float3(0.5, 0.5, 0.5);
				u_xlat10_9 = tex2D(_MaskTex, i.uv1).w;
				u_xlat16_1.xyz = -u_xlat10_9 * float3(5.36000013, 0.879795909, 0.0) + float3(1.0, 1.0, 1.0);
				u_xlat10_9 = tex2D(_MaskTexAlpha, i.uv1).w;
				u_xlat16_2 = u_xlat10_9 * 2.0 + -1.0;
				u_xlat16_2 = clamp(u_xlat16_2, 0.0, 1.0);
				u_xlat16_5 = u_xlat10_9 + u_xlat10_9;
				u_xlat16_5 = u_xlat16_5;
				u_xlat16_5 = clamp(u_xlat16_5, 0.0, 1.0);
				u_xlat16_1.xyz = u_xlat16_1.xyz + u_xlat16_2;
				finalColor.xyz = u_xlat0.xyz * u_xlat16_1.xyz;
				float depth = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.uv2));
				depth = Linear01Depth(depth);
				depth = 1.0f;
				finalColor.w = depth * u_xlat16_5;
				return finalColor;
            }
            ENDCG
        }
    }
}
