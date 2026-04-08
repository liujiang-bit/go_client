Shader "custom/city/building_move_arrow+21"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Bouncing ("Bounce Range", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always
        
        Tags { 
		    "CanUseSpriteAtlas" = "true" 
		    "IGNOREPROJECTOR" = "true" 
		    "PreviewType" = "Plane"             //Tag决定了材质面板的预览窗口如何显示模型，默认显示的是球体
		    "QUEUE" = "Transparent+21" 
		    "RenderType" = "Transparent" 
		}
		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha

        Pass
        {
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            float _Bouncing;

            v2f vert (appdata v)
            {
                v2f o;
                float waver = (((sin((_Time.y * 5.0)) / 20.0)+ 0.05) * _Bouncing) ;
				o.vertex.y =  o.vertex.y + waver; 	
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
