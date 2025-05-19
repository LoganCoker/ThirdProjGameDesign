Shader "Custom/FireBallShader" {

    Properties {
        _MainTexture("Texture", 2D) = "orange" {}
        _Color("Color", Color) = (1,1,1,1)
        _Speed("Play Speed", Range(0,3)) = 1
        _Wavy("Wavyness", Range(0,10)) = 1
    }

    SubShader {
        
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        Pass {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vertexFunc
            #pragma fragment fragmentFunc

            struct Input {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTexture;
            fixed4 _Color;
            float _Speed;
            float _Wavy;

            v2f vertexFunc(Input IN) {
                v2f OUT; 

                IN.vertex.x += sin(_Time.y * _Speed + IN.vertex.y * _Wavy)*0.1;


                OUT.position = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;

                return OUT;
            }

            fixed4 fragmentFunc(v2f IN) : SV_Target {
                fixed4 pixelColor = tex2D(_MainTexture, IN.uv);
                return pixelColor * _Color;
            }

            ENDCG
        }
    }
}