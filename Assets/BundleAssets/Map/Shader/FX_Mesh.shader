Shader "FX/FX_Mesh" {
    Properties {
        [Header(Model)]
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 2
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend Mode", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend Mode", Float) = 1
		[Enum(Off, 0, On, 1)] _ZWrite("ZWrite", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0


        _Color ("Color", Color) = (1,1,1,1)
		_ColorAffector("ColorAffector", Color) = (1,1,1,1)
        _Color_Emi ("Color_Emi", Float ) = 1
        _EmiTex ("EmiTex", 2D) = "white" {}
        _EmiTex_u ("EmiTex_u", Float ) = 0
        _EmiTex_v ("EmiTex_v", Float ) = 0
        _Noice_L ("Noice_L", Float ) = 0
        _Lerp_Noice ("Lerp_Noice", Float ) = 0
        _N01_x ("N01_x", Float ) = 0
        _N01_y ("N01_y", Float ) = 0
        _Noice ("Noice", 2D) = "white" {}
        _Noice_add ("Noice_add", Float ) = 0
        _N01_U ("N01_U", Float ) = 0
        _N01_V ("N01_V", Float ) = 0
        _N02_U ("N02_U", Float ) = 0
        _N02_V ("N02_V", Float ) = 0
        _Noice_A ("Noice_A", Float ) = 0
        _MainTex ("MainTex", 2D) = "bump" {}
        _Opacity_Main ("Opacity_Main", Float ) = 1



        _MainTex_u ("MainTex_u", Float ) = 0
        _MainTex_v ("MainTex_v", Float ) = 0




        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
			Cull [_Cull]
			Blend [_SrcBlend] [_DstBlend]
			ZWrite [_ZWrite]
			ZTest [_ZTest]



        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }

            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            //#pragma multi_compile_fwdbase
            //#pragma multi_compile_fog
            //#pragma only_renderers d3d9 d3d11 glcore gles 
            //#pragma target 3.0
            uniform float4 _Color;
		uniform float4 _ColorAffector;
            uniform float _N01_U;
            uniform float _N01_V;
            uniform float _Noice_L;
            uniform float _Noice_A;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _EmiTex; uniform float4 _EmiTex_ST;
            uniform float _Color_Emi;
            uniform float _Opacity_Main;
            uniform float _EmiTex_u;
            uniform float _EmiTex_v;
            uniform float _MainTex_u;
            uniform float _MainTex_v;
            uniform sampler2D _Noice; uniform float4 _Noice_ST;
            uniform float _Lerp_Noice;
            uniform float _N02_U;
            uniform float _N02_V;
            uniform float _N01_x;
            uniform float _N01_y;
            uniform float _Noice_add;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                //UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                //UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_5270 = _Time;
                float4 node_4795 = _Time;
                float2 node_1401 = ((i.uv0+float2(_N01_x,_N01_y))+float2((_N01_U*node_4795.r),(node_4795.r*_N01_V)));
                float4 node_9013 = tex2D(_Noice,TRANSFORM_TEX(node_1401, _Noice));
                float4 node_7662 = _Time;
                float2 node_6830 = (i.uv0+float2((_N02_U*node_7662.r),(node_7662.r*_N02_V)));
                float4 _Noice_copy = tex2D(_Noice,TRANSFORM_TEX(node_6830, _Noice));
                float node_1542 = (lerp(node_9013.r,_Noice_copy.r,_Lerp_Noice)+_Noice_add);
                float2 node_3731 = ((i.uv0+float2((_EmiTex_u*node_5270.r),(node_5270.r*_EmiTex_v)))+(node_1542*_Noice_L));
                float4 _EmiTex_var = tex2D(_EmiTex,TRANSFORM_TEX(node_3731, _EmiTex));
                float3 emissive = (_Color.rgb*_Color_Emi*_EmiTex_var.rgb*i.vertexColor.rgb);
                float3 finalColor = emissive;
                float4 node_4908 = _Time;
                float2 node_1449 = ((node_1542*_Noice_A)+(i.uv0+float2((_MainTex_u*node_4908.r),(node_4908.r*_MainTex_v))));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_1449, _MainTex));
                fixed4 finalRGBA = fixed4(finalColor,(_MainTex_var.a*_Opacity_Main*_Color.a*_ColorAffector.a*i.vertexColor.a));

                //UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
