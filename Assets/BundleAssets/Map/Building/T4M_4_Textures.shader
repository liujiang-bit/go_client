Shader "T4MShaders/ShaderModel2/Diffuse/T4M 4 Textures" {
Properties {
	_Layer1 ("Layer 1", 2D) = "white" {}
	_Layer2 ("Layer 2", 2D) = "white" {}
	_Layer3 ("Layer 3", 2D) = "white" {}
	_Layer4 ("Layer 4", 2D) = "white" {}
	_Tiling3("_Tiling4 x/y", Vector)=(1,1,0,0)
	_ControlRGBA ("Control (RGBA)", 2D) = "white" {}
	_MainTex ("Never Used", 2D) = "white" {}
}
                
SubShader {
	Tags {
   "SplatCount" = "4"
   "RenderType" = "Opaque"
	}
CGPROGRAM
#include "UnityCG.cginc"
#pragma surface surf Lambert //vertex:myvert finalcolor:mycolor
#pragma multi_compile_fog
#pragma exclude_renderers xbox360 ps3
//uniform half4 unity_FogStart;
//uniform half4 unity_FogEnd;


struct Input {
	float2 uv_ControlRGBA : TEXCOORD0;
	float2 uv_Layer1 : TEXCOORD1;
	float2 uv_Layer2 : TEXCOORD2;
	float2 uv_Layer3 : TEXCOORD3;
	float2 uv_Layer4 : TEXCOORD4;
   half fog;
};
 
sampler2D _ControlRGBA;
sampler2D _Layer1,_Layer2,_Layer3,_Layer4;
float4 _Tiling3;
 void myvert (inout appdata_full v, out Input data) {
      UNITY_INITIALIZE_OUTPUT(Input,data);

//      float4 hpos = mul(UNITY_MATRIX_MVP, v.vertex);
//       data.fog = min(1, dot(hpos.xy, hpos.xy)*0.1);
//      float pos =length(UnityObjectToViewPos(v.vertex).xyz);//length (_WorldSpaceCameraPos.xyz -  mul(_Object2World, v.vertex.xyz));//
//      float diff = unity_FogEnd.x - unity_FogStart.x;
//      float invDiff = 1.0f / diff;
//      data.fog =   clamp ((unity_FogEnd.x - pos)* invDiff   , 0.0, 1.0);//) * invDiff  
}
void mycolor (Input IN, SurfaceOutput o, inout fixed4 color) {

//  #ifdef UNITY_PASS_FORWARDADD
//  color = lerp(float4(0,0,0,0), color ,IN.fog);
//  #else
//  color = lerp(unity_FogColor, color ,IN.fog);
//   // UNITY_APPLY_FOG_COLOR(IN.fog, color, unity_FogColor);
//  #endif
}
void surf (Input IN, inout SurfaceOutput o) {
	fixed4 splat_ControlRGBA = tex2D (_ControlRGBA, IN.uv_ControlRGBA).rgba;
	splat_ControlRGBA.a = 1 - splat_ControlRGBA.r - splat_ControlRGBA.g - splat_ControlRGBA.b;
		
	fixed3 lay1 = tex2D (_Layer1, IN.uv_Layer1);
	fixed3 lay2 = tex2D (_Layer2, IN.uv_Layer2);
	fixed3 lay3 = tex2D (_Layer3, IN.uv_Layer3);
	fixed3 lay4 = tex2D (_Layer4, IN.uv_ControlRGBA*_Tiling3.xy);
	o.Alpha = 0.0;
	o.Albedo.rgb = (lay1 * splat_ControlRGBA.r + lay2 * splat_ControlRGBA.g + lay3 * splat_ControlRGBA.b + lay4 * splat_ControlRGBA.a);
}
ENDCG 
}
// Fallback to Diffuse
Fallback "Diffuse"
}
