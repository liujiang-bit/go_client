// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:False,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:32948,y:32759,varname:node_9361,prsc:2|emission-141-OUT,clip-7813-OUT;n:type:ShaderForge.SFN_Fresnel,id:8907,x:32203,y:32854,varname:node_8907,prsc:2|EXP-1504-OUT;n:type:ShaderForge.SFN_Slider,id:1504,x:31740,y:32972,ptovrint:False,ptlb:Rim_width,ptin:_Rim_width,varname:node_1504,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7754496,max:1;n:type:ShaderForge.SFN_Add,id:141,x:32570,y:32654,varname:node_141,prsc:2|A-2925-OUT,B-4443-OUT;n:type:ShaderForge.SFN_Tex2d,id:2729,x:32002,y:32571,ptovrint:False,ptlb:Main_tex,ptin:_Main_tex,varname:node_2729,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2925,x:32348,y:32598,varname:node_2925,prsc:2|A-4516-RGB,B-2729-RGB;n:type:ShaderForge.SFN_Color,id:4516,x:32053,y:32383,ptovrint:False,ptlb:Main_color,ptin:_Main_color,varname:node_4516,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_VertexColor,id:7754,x:32353,y:33369,varname:node_7754,prsc:2;n:type:ShaderForge.SFN_Step,id:7813,x:32596,y:33226,varname:node_7813,prsc:2|A-5487-R,B-7754-A;n:type:ShaderForge.SFN_Tex2d,id:5487,x:32130,y:33233,ptovrint:False,ptlb:Dissolution,ptin:_Dissolution,varname:node_5487,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1e55d2dd5cafb7c4780b46c659b3957f,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:9535,x:32031,y:33095,ptovrint:False,ptlb:Rim_Sthength,ptin:_Rim_Sthength,varname:node_9535,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2.191094,max:10;n:type:ShaderForge.SFN_Multiply,id:4443,x:32438,y:32935,varname:node_4443,prsc:2|A-8907-OUT,B-9535-OUT;proporder:4516-2729-5487-1504-9535;pass:END;sub:END;*/

Shader "Shader Forge/RimAddDissolution" {
    Properties {
        _Main_color ("Main_color", Color) = (0.5,0.5,0.5,1)
        _Main_tex ("Main_tex", 2D) = "white" {}
        _Dissolution ("Dissolution", 2D) = "white" {}
        _Rim_width ("Rim_width", Range(0, 1)) = 0.7754496
        _Rim_Sthength ("Rim_Sthength", Range(0, 10)) = 2.191094
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            uniform float _Rim_width;
            uniform sampler2D _Main_tex; uniform float4 _Main_tex_ST;
            uniform float4 _Main_color;
            uniform sampler2D _Dissolution; uniform float4 _Dissolution_ST;
            uniform float _Rim_Sthength;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _Dissolution_var = tex2D(_Dissolution,TRANSFORM_TEX(i.uv0, _Dissolution));
                clip(step(_Dissolution_var.r,i.vertexColor.a) - 0.5);
////// Lighting:
////// Emissive:
                float4 _Main_tex_var = tex2D(_Main_tex,TRANSFORM_TEX(i.uv0, _Main_tex));
                float3 emissive = ((_Main_color.rgb*_Main_tex_var.rgb)+(pow(1.0-max(0,dot(normalDirection, viewDirection)),_Rim_width)*_Rim_Sthength));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            uniform sampler2D _Dissolution; uniform float4 _Dissolution_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _Dissolution_var = tex2D(_Dissolution,TRANSFORM_TEX(i.uv0, _Dissolution));
                clip(step(_Dissolution_var.r,i.vertexColor.a) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
