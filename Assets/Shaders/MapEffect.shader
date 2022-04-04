Shader "Unlit/MapEffect"
{
    Properties
    {

        [HDR] _Colour("Colour", Color) = (0,1,0,1)
        [HDR] _Tint("Tint", Color) = (0,1,0,1)
          _MainTex("Color (RGB) Alpha (A)", 2D) = "white"
        

    }
        SubShader
        {
    
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            


            Pass
            {
                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha
                Cull Off
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
              
            //#pragma surface surf Standard fullforwardshadows vertex:vert

            #define TAU 6.283185
            #include "UnityCG.cginc"


 
            float4 _MainTex_ST;
            sampler2D _MainTex;
            float4 _Colour;
            float4 _Tint;

          



            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normals: NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float3 normal:TEXCOORD0;
                float2 uv : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };


            Interpolators vert(MeshData v)
            {

                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normals);
                o.uv = v.uv;
                return o;
            }
       
            float4 frag(Interpolators i) : SV_Target
            {
                float4 texColor;
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 uvsCentred = i.uv * 2 - 1;
                uvsCentred.x *= 19.2/10.8;
                float average = (col.x + col.y + col.z) / 3.0;
                texColor = float4(average, average, average, 1);
                float ratio = 19.2 / 10.8;
                uvsCentred.y *= (1.3);
                float fadeOut = 1;
                float yFade = (abs(uvsCentred.y) > 0.9) * lerp(1, 0, 4*(abs(uvsCentred.y) - 0.9));
                float xFade = (abs(uvsCentred.x) > 0.5*(ratio)) * lerp(1, 0, 4*(abs(uvsCentred.x) - (0.5*ratio)));
                if (yFade == 0) { yFade = 1; }
                if (xFade == 0) { xFade = 1; }
                if (abs(uvsCentred.x) > 1.17) { xFade = 0; }
                /*float initialFade = (length(uvsCentred)<1.2) * lerp(1, 0.85, length(uvsCentred) - 0.2);
                float secondaryFade = (length(uvsCentred)>= 1.2) * lerp(0.85, 0, length(uvsCentred) - 1.2);
                if (length(uvsCentred) < 1.2) {
                    fadeOut = initialFade;
                }
                if (length(uvsCentred) >= 1.2) {
                    fadeOut = secondaryFade;
                }*/

                float output = saturate(fadeOut);
               
                
                _Colour.a = saturate(xFade*(yFade/ratio))*1.8;
                return texColor * _Colour;
               
            }
            ENDCG
        }
        }
}
