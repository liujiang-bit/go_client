Shader "custom/landmap/water"
{
    Properties
    {
        _BumpMap ("Normals ", 2D) = "bump" {}
		_BumpTiling ("Bump Tiling", Vector) = (1,1,-2,3)
		_BaseColor ("Base color", Vector) = (0.54,0.95,0.99,0.5)
		_WorldLightDir ("Specular light direction", Vector) = (0,0.1,-0.5,0)
		_Shininess ("Shininess", Float) = 200
		_Gloss ("GLoss", Float) = 1
		_Speed ("wave speed", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent-23" "RenderType"="Transparent" }
        LOD 80
        
        Pass{
        
            Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
            ZWrite Off
    
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _BumpMap;
            float4 _BumpMap_ST;
            
            float4 _BumpTiling;
            float4 _BaseColor;
            float4 _WorldLightDir;
            float _Shininess; 
            float _Gloss;
            float4 _Speed;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;   
                float4 color : COLOR;
                float3 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                
                float4 tmpVar3;
                float3 worldPos = mul(UNITY_MATRIX_M, v.vertex);
                
                o.uv0 = normalize(worldPos.xyz - _WorldSpaceCameraPos);
                
                tmpVar3.xy = ((worldPos.xz * 2) + (_Speed.xy * _Time.x)) * _BumpTiling.xy;
                tmpVar3.zw = (worldPos.xz + (_Speed.zw * _Time.x)) * _BumpTiling.zw;
                
                o.uv1 = tmpVar3;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                
                return o;
            }
            
            float4 frag(v2f i) : SV_Target
            {
                float3 tmpVar2;
                tmpVar2 = (tex2D(_BumpMap, i.uv1.xy).xyz * 2 - 1) + (tex2D(_BumpMap, i.uv1.zw).xyz * 2 - 1) / 2;
                
                float4 tmpVar3;
                float dotValue = dot(tmpVar2, -(normalize(_WorldLightDir.xyz + i.uv0.xyz)));
                
                
                tmpVar3 = _BaseColor + (max(0, pow(max(0, dotValue), _Shininess)) * _Gloss);
                float4 col = tmpVar3;
                col.a = col.a * i.color.a;
                return col;
            }            
            ENDCG
        }
    }
    FallBack "Diffuse"
}
