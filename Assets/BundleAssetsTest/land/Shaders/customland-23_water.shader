Shader "custom/land/-23_water"
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
		_Near ("Near", float) = 0.3
		_Far ("Far", float) = 3.5
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
            fixed _Near;
            fixed _Far;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;   
                float4 color : COLOR;
                float4 uv1 : TEXCOORD1;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                
                float4 uvOffset;
                
                uvOffset.xy = ((v.vertex.xz * 2) + (_Speed.xy * _Time.x)) * _BumpTiling.xy;
                uvOffset.zw = (v.vertex.xz + (_Speed.zw * _Time.x)) * _BumpTiling.zw;
                o.uv1 = uvOffset;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                
                return o;
            }
            
            float4 frag(v2f i) : SV_Target
            {
                float3 normalColor;
                normalColor = (tex2D(_BumpMap, i.uv1.xy).xyz * 2 - 1) + (tex2D(_BumpMap, i.uv1.zw).xyz * 2 - 1) / 2;
                
                fixed zDis = max(0, _WorldSpaceCameraPos.y / 70);
                fixed factor = lerp(_Near, _Far, zDis);
                
                float normalY = normalColor.y * factor;
                float4 col = _BaseColor + (max(0, pow(max(0, normalY), _Shininess)) * _Gloss);
                col.a = col.a * i.color.a;
                col.rgb = saturate(col.rgb);
                col.a = saturate(col.a);
                return col;
            }            
            ENDCG
        }
    }
    FallBack "Diffuse"
}
