Shader "custom/map/-23_water_with_cubeMap" {
	Properties {
		_BumpMap ("Normals ", 2D) = "bump" {}
		_Cube ("Reflection Cubemap", Cube) = "_Skybox" {}
		_BumpTiling ("Bump Tiling", Vector) = (1,1,-2,3)
		_BaseColor ("Base color", Vector) = (0.54,0.95,0.99,0.5)
		_WorldLightDir ("Specular light direction", Vector) = (0,0.1,-0.5,0)
		_Shininess ("Shininess", Float) = 200
		_Gloss ("GLoss", Float) = 1
		_Speed ("wave speed", Vector) = (0,0,0,0)
		_ReflectAmount ("_ReflectAmount", Range(0.01, 1)) = 0.2
	}
	SubShader {
		LOD 80
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent-23" "RenderType" = "Transparent" }
		Pass {
			LOD 80
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent-23" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			Stencil {
				Ref 10
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 7884
			Program "vp" {
				SubProgram "gles hw_tier00 " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_WorldToObject;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _BumpTiling;
					uniform highp vec4 _Speed;
					varying highp vec4 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_Color;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3 = (unity_ObjectToWorld * _glesVertex);
					  tmpvar_2.xy = ((tmpvar_3.xz + (_Speed.xy * _Time.x)) * _BumpTiling.xy);
					  tmpvar_2.zw = ((tmpvar_3.xz + (_Speed.zw * _Time.x)) * _BumpTiling.zw);
					  tmpvar_1.xyz = normalize((tmpvar_3.xyz - _WorldSpaceCameraPos));
					  tmpvar_1.w = 0.0;
					  highp vec4 tmpvar_4;
					  tmpvar_4.w = 1.0;
					  tmpvar_4.xyz = _glesVertex.xyz;
					  highp mat3 tmpvar_5;
					  tmpvar_5[0] = unity_WorldToObject[0].xyz;
					  tmpvar_5[1] = unity_WorldToObject[1].xyz;
					  tmpvar_5[2] = unity_WorldToObject[2].xyz;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize(normalize((_glesNormal * tmpvar_5)));
					  highp vec3 I_7;
					  I_7 = (tmpvar_3.xyz - _WorldSpaceCameraPos);
					  gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_4));
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (I_7 - (2.0 * (
					    dot (tmpvar_6, I_7)
					   * tmpvar_6)));
					  xlv_Color = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _BumpMap;
					uniform lowp samplerCube _Cube;
					uniform highp vec4 _BaseColor;
					uniform highp float _Shininess;
					uniform highp vec4 _WorldLightDir;
					uniform highp float _Gloss;
					uniform lowp float _ReflectAmount;
					varying highp vec4 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_Color;
					void main ()
					{
					  lowp vec4 c_1;
					  lowp vec3 tmpvar_2;
					  tmpvar_2 = (((
					    (texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0)
					   - 1.0) + (
					    (texture2D (_BumpMap, xlv_TEXCOORD1.zw).xyz * 2.0)
					   - 1.0)) / 2.0);
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = textureCube (_Cube, xlv_TEXCOORD2);
					  highp vec4 tmpvar_4;
					  tmpvar_4 = (mix (_BaseColor, tmpvar_3, vec4(_ReflectAmount)) + (max (0.0, 
					    pow (max (0.0, dot (tmpvar_2, -(
					      normalize((_WorldLightDir.xyz + xlv_TEXCOORD0.xyz))
					    ))), _Shininess)
					  ) * _Gloss));
					  c_1 = tmpvar_4;
					  c_1.w = (c_1.w * xlv_Color.x);
					  gl_FragData[0] = c_1;
					}
					
					
					#endif"
				}
				SubProgram "gles hw_tier01 " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_WorldToObject;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _BumpTiling;
					uniform highp vec4 _Speed;
					varying highp vec4 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_Color;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3 = (unity_ObjectToWorld * _glesVertex);
					  tmpvar_2.xy = ((tmpvar_3.xz + (_Speed.xy * _Time.x)) * _BumpTiling.xy);
					  tmpvar_2.zw = ((tmpvar_3.xz + (_Speed.zw * _Time.x)) * _BumpTiling.zw);
					  tmpvar_1.xyz = normalize((tmpvar_3.xyz - _WorldSpaceCameraPos));
					  tmpvar_1.w = 0.0;
					  highp vec4 tmpvar_4;
					  tmpvar_4.w = 1.0;
					  tmpvar_4.xyz = _glesVertex.xyz;
					  highp mat3 tmpvar_5;
					  tmpvar_5[0] = unity_WorldToObject[0].xyz;
					  tmpvar_5[1] = unity_WorldToObject[1].xyz;
					  tmpvar_5[2] = unity_WorldToObject[2].xyz;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize(normalize((_glesNormal * tmpvar_5)));
					  highp vec3 I_7;
					  I_7 = (tmpvar_3.xyz - _WorldSpaceCameraPos);
					  gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_4));
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (I_7 - (2.0 * (
					    dot (tmpvar_6, I_7)
					   * tmpvar_6)));
					  xlv_Color = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _BumpMap;
					uniform lowp samplerCube _Cube;
					uniform highp vec4 _BaseColor;
					uniform highp float _Shininess;
					uniform highp vec4 _WorldLightDir;
					uniform highp float _Gloss;
					uniform lowp float _ReflectAmount;
					varying highp vec4 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_Color;
					void main ()
					{
					  lowp vec4 c_1;
					  lowp vec3 tmpvar_2;
					  tmpvar_2 = (((
					    (texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0)
					   - 1.0) + (
					    (texture2D (_BumpMap, xlv_TEXCOORD1.zw).xyz * 2.0)
					   - 1.0)) / 2.0);
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = textureCube (_Cube, xlv_TEXCOORD2);
					  highp vec4 tmpvar_4;
					  tmpvar_4 = (mix (_BaseColor, tmpvar_3, vec4(_ReflectAmount)) + (max (0.0, 
					    pow (max (0.0, dot (tmpvar_2, -(
					      normalize((_WorldLightDir.xyz + xlv_TEXCOORD0.xyz))
					    ))), _Shininess)
					  ) * _Gloss));
					  c_1 = tmpvar_4;
					  c_1.w = (c_1.w * xlv_Color.x);
					  gl_FragData[0] = c_1;
					}
					
					
					#endif"
				}
				SubProgram "gles hw_tier02 " {
					"!!!!GLES
					#version 100
					
					#ifdef VERTEX
					attribute vec4 _glesVertex;
					attribute vec4 _glesColor;
					attribute vec3 _glesNormal;
					uniform highp vec4 _Time;
					uniform highp vec3 _WorldSpaceCameraPos;
					uniform highp mat4 unity_ObjectToWorld;
					uniform highp mat4 unity_WorldToObject;
					uniform highp mat4 unity_MatrixVP;
					uniform highp vec4 _BumpTiling;
					uniform highp vec4 _Speed;
					varying highp vec4 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_Color;
					void main ()
					{
					  highp vec4 tmpvar_1;
					  highp vec4 tmpvar_2;
					  highp vec4 tmpvar_3;
					  tmpvar_3 = (unity_ObjectToWorld * _glesVertex);
					  tmpvar_2.xy = ((tmpvar_3.xz + (_Speed.xy * _Time.x)) * _BumpTiling.xy);
					  tmpvar_2.zw = ((tmpvar_3.xz + (_Speed.zw * _Time.x)) * _BumpTiling.zw);
					  tmpvar_1.xyz = normalize((tmpvar_3.xyz - _WorldSpaceCameraPos));
					  tmpvar_1.w = 0.0;
					  highp vec4 tmpvar_4;
					  tmpvar_4.w = 1.0;
					  tmpvar_4.xyz = _glesVertex.xyz;
					  highp mat3 tmpvar_5;
					  tmpvar_5[0] = unity_WorldToObject[0].xyz;
					  tmpvar_5[1] = unity_WorldToObject[1].xyz;
					  tmpvar_5[2] = unity_WorldToObject[2].xyz;
					  highp vec3 tmpvar_6;
					  tmpvar_6 = normalize(normalize((_glesNormal * tmpvar_5)));
					  highp vec3 I_7;
					  I_7 = (tmpvar_3.xyz - _WorldSpaceCameraPos);
					  gl_Position = (unity_MatrixVP * (unity_ObjectToWorld * tmpvar_4));
					  xlv_TEXCOORD0 = tmpvar_1;
					  xlv_TEXCOORD1 = tmpvar_2;
					  xlv_TEXCOORD2 = (I_7 - (2.0 * (
					    dot (tmpvar_6, I_7)
					   * tmpvar_6)));
					  xlv_Color = _glesColor;
					}
					
					
					#endif
					#ifdef FRAGMENT
					uniform sampler2D _BumpMap;
					uniform lowp samplerCube _Cube;
					uniform highp vec4 _BaseColor;
					uniform highp float _Shininess;
					uniform highp vec4 _WorldLightDir;
					uniform highp float _Gloss;
					uniform lowp float _ReflectAmount;
					varying highp vec4 xlv_TEXCOORD0;
					varying highp vec4 xlv_TEXCOORD1;
					varying highp vec3 xlv_TEXCOORD2;
					varying lowp vec4 xlv_Color;
					void main ()
					{
					  lowp vec4 c_1;
					  lowp vec3 tmpvar_2;
					  tmpvar_2 = (((
					    (texture2D (_BumpMap, xlv_TEXCOORD1.xy).xyz * 2.0)
					   - 1.0) + (
					    (texture2D (_BumpMap, xlv_TEXCOORD1.zw).xyz * 2.0)
					   - 1.0)) / 2.0);
					  lowp vec4 tmpvar_3;
					  tmpvar_3 = textureCube (_Cube, xlv_TEXCOORD2);
					  highp vec4 tmpvar_4;
					  tmpvar_4 = (mix (_BaseColor, tmpvar_3, vec4(_ReflectAmount)) + (max (0.0, 
					    pow (max (0.0, dot (tmpvar_2, -(
					      normalize((_WorldLightDir.xyz + xlv_TEXCOORD0.xyz))
					    ))), _Shininess)
					  ) * _Gloss));
					  c_1 = tmpvar_4;
					  c_1.w = (c_1.w * xlv_Color.x);
					  gl_FragData[0] = c_1;
					}
					
					
					#endif"
				}
				SubProgram "gles3 hw_tier00 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 _Time;
					uniform 	vec3 _WorldSpaceCameraPos;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_WorldToObject[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _BumpTiling;
					uniform 	vec4 _Speed;
					in highp vec4 in_POSITION0;
					in highp vec3 in_NORMAL0;
					in mediump vec4 in_COLOR0;
					out highp vec4 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					out highp vec3 vs_TEXCOORD2;
					out mediump vec4 vs_Color0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					float u_xlat6;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    u_xlat0.xyz = in_POSITION0.yyy * hlslcc_mtx4x4unity_ObjectToWorld[1].xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4unity_ObjectToWorld[0].xyz * in_POSITION0.xxx + u_xlat0.xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4unity_ObjectToWorld[2].xyz * in_POSITION0.zzz + u_xlat0.xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat1.xyz = u_xlat0.xyz + (-_WorldSpaceCameraPos.xyz);
					    u_xlat6 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat6 = inversesqrt(u_xlat6);
					    vs_TEXCOORD0.xyz = vec3(u_xlat6) * u_xlat1.xyz;
					    vs_TEXCOORD0.w = 0.0;
					    u_xlat1 = _Speed * _Time.xxxx + u_xlat0.xzxz;
					    u_xlat0.xyz = (-u_xlat0.xyz) + _WorldSpaceCameraPos.xyz;
					    vs_TEXCOORD1 = u_xlat1 * _BumpTiling;
					    u_xlat1.x = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[0].xyz);
					    u_xlat1.y = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[1].xyz);
					    u_xlat1.z = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[2].xyz);
					    u_xlat6 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat6 = inversesqrt(u_xlat6);
					    u_xlat1.xyz = vec3(u_xlat6) * u_xlat1.xyz;
					    u_xlat6 = dot((-u_xlat0.xyz), u_xlat1.xyz);
					    u_xlat6 = u_xlat6 + u_xlat6;
					    vs_TEXCOORD2.xyz = u_xlat1.xyz * (-vec3(u_xlat6)) + (-u_xlat0.xyz);
					    vs_Color0 = in_COLOR0;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _BaseColor;
					uniform 	float _Shininess;
					uniform 	vec4 _WorldLightDir;
					uniform 	float _Gloss;
					uniform 	mediump float _ReflectAmount;
					uniform lowp sampler2D _BumpMap;
					uniform lowp samplerCube _Cube;
					in highp vec4 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					in highp vec3 vs_TEXCOORD2;
					in mediump vec4 vs_Color0;
					layout(location = 0) out mediump vec4 SV_Target0;
					vec4 u_xlat0;
					lowp vec3 u_xlat10_0;
					vec4 u_xlat1;
					mediump vec3 u_xlat16_1;
					lowp vec4 u_xlat10_1;
					float u_xlat6;
					void main()
					{
					    u_xlat10_0.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
					    u_xlat16_1.xyz = u_xlat10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
					    u_xlat10_0.xyz = texture(_BumpMap, vs_TEXCOORD1.zw).xyz;
					    u_xlat16_1.xyz = u_xlat10_0.xyz * vec3(2.0, 2.0, 2.0) + u_xlat16_1.xyz;
					    u_xlat16_1.xyz = u_xlat16_1.xyz + vec3(-1.0, -1.0, -1.0);
					    u_xlat16_1.xyz = u_xlat16_1.xyz * vec3(0.5, 0.5, 0.5);
					    u_xlat0.xyz = vs_TEXCOORD0.xyz + _WorldLightDir.xyz;
					    u_xlat6 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat6 = inversesqrt(u_xlat6);
					    u_xlat0.xyz = vec3(u_xlat6) * u_xlat0.xyz;
					    u_xlat0.x = dot(u_xlat16_1.xyz, (-u_xlat0.xyz));
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat0.x = log2(u_xlat0.x);
					    u_xlat0.x = u_xlat0.x * _Shininess;
					    u_xlat0.x = exp2(u_xlat0.x);
					    u_xlat10_1 = texture(_Cube, vs_TEXCOORD2.xyz);
					    u_xlat1 = u_xlat10_1 + (-_BaseColor);
					    u_xlat1 = vec4(_ReflectAmount) * u_xlat1 + _BaseColor;
					    u_xlat0 = u_xlat0.xxxx * vec4(_Gloss) + u_xlat1;
					    SV_Target0.w = u_xlat0.w * vs_Color0.x;
					    SV_Target0.xyz = u_xlat0.xyz;
					    return;
					}
					
					#endif"
				}
				SubProgram "gles3 hw_tier01 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 _Time;
					uniform 	vec3 _WorldSpaceCameraPos;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_WorldToObject[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _BumpTiling;
					uniform 	vec4 _Speed;
					in highp vec4 in_POSITION0;
					in highp vec3 in_NORMAL0;
					in mediump vec4 in_COLOR0;
					out highp vec4 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					out highp vec3 vs_TEXCOORD2;
					out mediump vec4 vs_Color0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					float u_xlat6;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    u_xlat0.xyz = in_POSITION0.yyy * hlslcc_mtx4x4unity_ObjectToWorld[1].xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4unity_ObjectToWorld[0].xyz * in_POSITION0.xxx + u_xlat0.xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4unity_ObjectToWorld[2].xyz * in_POSITION0.zzz + u_xlat0.xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat1.xyz = u_xlat0.xyz + (-_WorldSpaceCameraPos.xyz);
					    u_xlat6 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat6 = inversesqrt(u_xlat6);
					    vs_TEXCOORD0.xyz = vec3(u_xlat6) * u_xlat1.xyz;
					    vs_TEXCOORD0.w = 0.0;
					    u_xlat1 = _Speed * _Time.xxxx + u_xlat0.xzxz;
					    u_xlat0.xyz = (-u_xlat0.xyz) + _WorldSpaceCameraPos.xyz;
					    vs_TEXCOORD1 = u_xlat1 * _BumpTiling;
					    u_xlat1.x = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[0].xyz);
					    u_xlat1.y = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[1].xyz);
					    u_xlat1.z = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[2].xyz);
					    u_xlat6 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat6 = inversesqrt(u_xlat6);
					    u_xlat1.xyz = vec3(u_xlat6) * u_xlat1.xyz;
					    u_xlat6 = dot((-u_xlat0.xyz), u_xlat1.xyz);
					    u_xlat6 = u_xlat6 + u_xlat6;
					    vs_TEXCOORD2.xyz = u_xlat1.xyz * (-vec3(u_xlat6)) + (-u_xlat0.xyz);
					    vs_Color0 = in_COLOR0;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _BaseColor;
					uniform 	float _Shininess;
					uniform 	vec4 _WorldLightDir;
					uniform 	float _Gloss;
					uniform 	mediump float _ReflectAmount;
					uniform lowp sampler2D _BumpMap;
					uniform lowp samplerCube _Cube;
					in highp vec4 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					in highp vec3 vs_TEXCOORD2;
					in mediump vec4 vs_Color0;
					layout(location = 0) out mediump vec4 SV_Target0;
					vec4 u_xlat0;
					lowp vec3 u_xlat10_0;
					vec4 u_xlat1;
					mediump vec3 u_xlat16_1;
					lowp vec4 u_xlat10_1;
					float u_xlat6;
					void main()
					{
					    u_xlat10_0.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
					    u_xlat16_1.xyz = u_xlat10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
					    u_xlat10_0.xyz = texture(_BumpMap, vs_TEXCOORD1.zw).xyz;
					    u_xlat16_1.xyz = u_xlat10_0.xyz * vec3(2.0, 2.0, 2.0) + u_xlat16_1.xyz;
					    u_xlat16_1.xyz = u_xlat16_1.xyz + vec3(-1.0, -1.0, -1.0);
					    u_xlat16_1.xyz = u_xlat16_1.xyz * vec3(0.5, 0.5, 0.5);
					    u_xlat0.xyz = vs_TEXCOORD0.xyz + _WorldLightDir.xyz;
					    u_xlat6 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat6 = inversesqrt(u_xlat6);
					    u_xlat0.xyz = vec3(u_xlat6) * u_xlat0.xyz;
					    u_xlat0.x = dot(u_xlat16_1.xyz, (-u_xlat0.xyz));
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat0.x = log2(u_xlat0.x);
					    u_xlat0.x = u_xlat0.x * _Shininess;
					    u_xlat0.x = exp2(u_xlat0.x);
					    u_xlat10_1 = texture(_Cube, vs_TEXCOORD2.xyz);
					    u_xlat1 = u_xlat10_1 + (-_BaseColor);
					    u_xlat1 = vec4(_ReflectAmount) * u_xlat1 + _BaseColor;
					    u_xlat0 = u_xlat0.xxxx * vec4(_Gloss) + u_xlat1;
					    SV_Target0.w = u_xlat0.w * vs_Color0.x;
					    SV_Target0.xyz = u_xlat0.xyz;
					    return;
					}
					
					#endif"
				}
				SubProgram "gles3 hw_tier02 " {
					"!!!!GLES3
					#ifdef VERTEX
					#version 300 es
					
					uniform 	vec4 _Time;
					uniform 	vec3 _WorldSpaceCameraPos;
					uniform 	vec4 hlslcc_mtx4x4unity_ObjectToWorld[4];
					uniform 	vec4 hlslcc_mtx4x4unity_WorldToObject[4];
					uniform 	vec4 hlslcc_mtx4x4unity_MatrixVP[4];
					uniform 	vec4 _BumpTiling;
					uniform 	vec4 _Speed;
					in highp vec4 in_POSITION0;
					in highp vec3 in_NORMAL0;
					in mediump vec4 in_COLOR0;
					out highp vec4 vs_TEXCOORD0;
					out highp vec4 vs_TEXCOORD1;
					out highp vec3 vs_TEXCOORD2;
					out mediump vec4 vs_Color0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					float u_xlat6;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * hlslcc_mtx4x4unity_ObjectToWorld[1];
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = hlslcc_mtx4x4unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + hlslcc_mtx4x4unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * hlslcc_mtx4x4unity_MatrixVP[1];
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = hlslcc_mtx4x4unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = hlslcc_mtx4x4unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    u_xlat0.xyz = in_POSITION0.yyy * hlslcc_mtx4x4unity_ObjectToWorld[1].xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4unity_ObjectToWorld[0].xyz * in_POSITION0.xxx + u_xlat0.xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4unity_ObjectToWorld[2].xyz * in_POSITION0.zzz + u_xlat0.xyz;
					    u_xlat0.xyz = hlslcc_mtx4x4unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat1.xyz = u_xlat0.xyz + (-_WorldSpaceCameraPos.xyz);
					    u_xlat6 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat6 = inversesqrt(u_xlat6);
					    vs_TEXCOORD0.xyz = vec3(u_xlat6) * u_xlat1.xyz;
					    vs_TEXCOORD0.w = 0.0;
					    u_xlat1 = _Speed * _Time.xxxx + u_xlat0.xzxz;
					    u_xlat0.xyz = (-u_xlat0.xyz) + _WorldSpaceCameraPos.xyz;
					    vs_TEXCOORD1 = u_xlat1 * _BumpTiling;
					    u_xlat1.x = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[0].xyz);
					    u_xlat1.y = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[1].xyz);
					    u_xlat1.z = dot(in_NORMAL0.xyz, hlslcc_mtx4x4unity_WorldToObject[2].xyz);
					    u_xlat6 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat6 = inversesqrt(u_xlat6);
					    u_xlat1.xyz = vec3(u_xlat6) * u_xlat1.xyz;
					    u_xlat6 = dot((-u_xlat0.xyz), u_xlat1.xyz);
					    u_xlat6 = u_xlat6 + u_xlat6;
					    vs_TEXCOORD2.xyz = u_xlat1.xyz * (-vec3(u_xlat6)) + (-u_xlat0.xyz);
					    vs_Color0 = in_COLOR0;
					    return;
					}
					
					#endif
					#ifdef FRAGMENT
					#version 300 es
					
					precision highp int;
					uniform 	vec4 _BaseColor;
					uniform 	float _Shininess;
					uniform 	vec4 _WorldLightDir;
					uniform 	float _Gloss;
					uniform 	mediump float _ReflectAmount;
					uniform lowp sampler2D _BumpMap;
					uniform lowp samplerCube _Cube;
					in highp vec4 vs_TEXCOORD0;
					in highp vec4 vs_TEXCOORD1;
					in highp vec3 vs_TEXCOORD2;
					in mediump vec4 vs_Color0;
					layout(location = 0) out mediump vec4 SV_Target0;
					vec4 u_xlat0;
					lowp vec3 u_xlat10_0;
					vec4 u_xlat1;
					mediump vec3 u_xlat16_1;
					lowp vec4 u_xlat10_1;
					float u_xlat6;
					void main()
					{
					    u_xlat10_0.xyz = texture(_BumpMap, vs_TEXCOORD1.xy).xyz;
					    u_xlat16_1.xyz = u_xlat10_0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
					    u_xlat10_0.xyz = texture(_BumpMap, vs_TEXCOORD1.zw).xyz;
					    u_xlat16_1.xyz = u_xlat10_0.xyz * vec3(2.0, 2.0, 2.0) + u_xlat16_1.xyz;
					    u_xlat16_1.xyz = u_xlat16_1.xyz + vec3(-1.0, -1.0, -1.0);
					    u_xlat16_1.xyz = u_xlat16_1.xyz * vec3(0.5, 0.5, 0.5);
					    u_xlat0.xyz = vs_TEXCOORD0.xyz + _WorldLightDir.xyz;
					    u_xlat6 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat6 = inversesqrt(u_xlat6);
					    u_xlat0.xyz = vec3(u_xlat6) * u_xlat0.xyz;
					    u_xlat0.x = dot(u_xlat16_1.xyz, (-u_xlat0.xyz));
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat0.x = log2(u_xlat0.x);
					    u_xlat0.x = u_xlat0.x * _Shininess;
					    u_xlat0.x = exp2(u_xlat0.x);
					    u_xlat10_1 = texture(_Cube, vs_TEXCOORD2.xyz);
					    u_xlat1 = u_xlat10_1 + (-_BaseColor);
					    u_xlat1 = vec4(_ReflectAmount) * u_xlat1 + _BaseColor;
					    u_xlat0 = u_xlat0.xxxx * vec4(_Gloss) + u_xlat1;
					    SV_Target0.w = u_xlat0.w * vs_Color0.x;
					    SV_Target0.xyz = u_xlat0.xyz;
					    return;
					}
					
					#endif"
				}
			}
			Program "fp" {
				SubProgram "gles hw_tier00 " {
					"!!!!GLES"
				}
				SubProgram "gles hw_tier01 " {
					"!!!!GLES"
				}
				SubProgram "gles hw_tier02 " {
					"!!!!GLES"
				}
				SubProgram "gles3 hw_tier00 " {
					"!!!!GLES3"
				}
				SubProgram "gles3 hw_tier01 " {
					"!!!!GLES3"
				}
				SubProgram "gles3 hw_tier02 " {
					"!!!!GLES3"
				}
			}
		}
	}
}