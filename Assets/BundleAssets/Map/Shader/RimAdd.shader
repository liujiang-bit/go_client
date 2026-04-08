// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:False,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:32948,y:32759,varname:node_9361,prsc:2|emission-141-OUT,alpha-4700-OUT;n:type:ShaderForge.SFN_Fresnel,id:8907,x:32196,y:32854,varname:node_8907,prsc:2|NRM-181-OUT,EXP-1504-OUT;n:type:ShaderForge.SFN_Slider,id:1504,x:31730,y:32945,ptovrint:False,ptlb:Rim_width,ptin:_Rim_width,varname:node_1504,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8888889,max:1;n:type:ShaderForge.SFN_Add,id:141,x:32505,y:32689,varname:node_141,prsc:2|A-2925-OUT,B-2452-OUT;n:type:ShaderForge.SFN_Tex2d,id:2729,x:32002,y:32571,ptovrint:False,ptlb:Main_tex,ptin:_Main_tex,varname:node_2729,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:9368,x:32092,y:33020,ptovrint:False,ptlb:Rim_color,ptin:_Rim_color,varname:node_9368,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:2452,x:32390,y:32914,varname:node_2452,prsc:2|A-8907-OUT,B-9368-RGB;n:type:ShaderForge.SFN_Multiply,id:2925,x:32348,y:32598,varname:node_2925,prsc:2|A-4516-RGB,B-2729-RGB;n:type:ShaderForge.SFN_Color,id:4516,x:32053,y:32383,ptovrint:False,ptlb:Main_color,ptin:_Main_color,varname:node_4516,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:4700,x:32754,y:32992,varname:node_4700,prsc:2|A-8907-OUT,B-4516-A;n:type:ShaderForge.SFN_NormalVector,id:181,x:31940,y:32798,prsc:2,pt:False;proporder:4516-9368-2729-1504;pass:END;sub:END;*/

Shader "Shader Forge/RimAdd" {
    Properties {
        _Main_color ("Main_color", Color) = (0.5,0.5,0.5,1)
        _Rim_color ("Rim_color", Color) = (0.5,0.5,0.5,1)
        _Main_tex ("Main_tex", 2D) = "white" {}
        _Rim_width ("Rim_width", Range(0, 1)) = 0.8888889
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
            Blend SrcAlpha OneMinusSrcAlpha
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            uniform float _Rim_width;
            uniform sampler2D _Main_tex; uniform float4 _Main_tex_ST;
            uniform float4 _Rim_color;
            uniform float4 _Main_color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 _Main_tex_var = tex2D(_Main_tex,TRANSFORM_TEX(i.uv0, _Main_tex));
                float node_8907 = pow(1.0-max(0,dot(i.normalDir, viewDirection)),_Rim_width);
                float3 emissive = ((_Main_color.rgb*_Main_tex_var.rgb)+(node_8907*_Rim_color.rgb));
                float3 finalColor = emissive;
                return fixed4(finalColor,(node_8907*_Main_color.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
