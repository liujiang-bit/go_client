// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.09385066,fgcg:0.0932634,fgcb:0.1102941,fgca:1,fgde:0.04,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33620,y:32749,varname:node_9361,prsc:2|alpha-3851-OUT,refract-8254-OUT;n:type:ShaderForge.SFN_Tex2d,id:6489,x:32787,y:32949,ptovrint:False,ptlb:Main_Tex,ptin:_Main_Tex,varname:_Main_Tex,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6537-OUT;n:type:ShaderForge.SFN_Append,id:4000,x:33073,y:32887,varname:node_4000,prsc:2|A-6489-R,B-6489-G;n:type:ShaderForge.SFN_Multiply,id:8254,x:33317,y:33160,varname:node_8254,prsc:2|A-4000-OUT,B-6489-A,C-4185-OUT,D-5009-A;n:type:ShaderForge.SFN_Slider,id:4185,x:32934,y:33261,ptovrint:False,ptlb:Indensity,ptin:_Indensity,varname:_Indensity,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1265353,max:1;n:type:ShaderForge.SFN_VertexColor,id:5009,x:33126,y:33345,varname:node_5009,prsc:2;n:type:ShaderForge.SFN_Vector1,id:3851,x:33125,y:32801,varname:node_3851,prsc:2,v1:0;n:type:ShaderForge.SFN_Time,id:863,x:31807,y:32747,varname:node_863,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9585,x:32091,y:32876,varname:node_9585,prsc:2|A-863-T,B-4932-OUT;n:type:ShaderForge.SFN_Slider,id:4932,x:31729,y:33020,ptovrint:False,ptlb:U_speed,ptin:_U_speed,varname:node_5987,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-10,cur:0,max:10;n:type:ShaderForge.SFN_Multiply,id:5426,x:32111,y:33085,varname:node_5426,prsc:2|A-863-T,B-5023-OUT;n:type:ShaderForge.SFN_Slider,id:5023,x:31741,y:33190,ptovrint:False,ptlb:V_speed,ptin:_V_speed,varname:_node_5987_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-10,cur:0,max:10;n:type:ShaderForge.SFN_Append,id:8536,x:32368,y:32955,varname:node_8536,prsc:2|A-9585-OUT,B-5426-OUT;n:type:ShaderForge.SFN_Add,id:6537,x:32342,y:32663,varname:node_6537,prsc:2|A-8215-UVOUT,B-8536-OUT;n:type:ShaderForge.SFN_TexCoord,id:8215,x:32106,y:32553,varname:node_8215,prsc:2,uv:0,uaff:False;proporder:6489-4185-4932-5023;pass:END;sub:END;*/

Shader "Effect/HeatDissolve" {
    Properties {
        _Main_Tex ("Main_Tex", 2D) = "white" {}
        _Indensity ("Indensity", Range(0, 1)) = 0.1265353
        _U_speed ("U_speed", Range(-10, 10)) = 0
        _V_speed ("V_speed", Range(-10, 10)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _Main_Tex; uniform float4 _Main_Tex_ST;
            uniform fixed _Indensity;
            uniform float _U_speed;
            uniform float _V_speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 node_863 = _Time;
                float2 node_6537 = (i.uv0+float2((node_863.g*_U_speed),(node_863.g*_V_speed)));
                fixed4 _Main_Tex_var = tex2D(_Main_Tex,TRANSFORM_TEX(node_6537, _Main_Tex));
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (float2(_Main_Tex_var.r,_Main_Tex_var.g)*_Main_Tex_var.a*_Indensity*i.vertexColor.a);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
                float3 finalColor = 0;
                return fixed4(lerp(sceneColor.rgb, finalColor,0.0),1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
