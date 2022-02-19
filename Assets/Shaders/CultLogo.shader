Shader "Unlit/CultLogo"
{
    Properties
    {

        [HDR] _ColourA("Colour A", Color) = (0,1,0,1)
        _Color("Tint", Color) = (0, 0, 0, 1)
        _MainTex("Texture", 2D) = "white" {}
        [HDR]_ColourB("Colour B", Color) = (1,1,0,1)
         _Transparency("Float with range", Range(0.0, 1.0)) = 0
        _ColourStart("Colour Start" , Range(0,1)) = 0
        _ColourEnd("Colour End" , Range(0,1)) = 1

    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
            LOD 100

            Pass
            {
                ZWrite Off
                Blend One One
                Cull Off
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
            //#pragma surface surf Standard fullforwardshadows vertex:vert

            #define TAU 6.283185
            #include "UnityCG.cginc"


            float4 _ColourA;
            float4 _ColourB;
            float _ColourStart;
            float _ColourEnd;
            float4 _MainTex_ST;
            fixed4 _Color;
            sampler2D _MainTex;
            float _Transparency;
          



            float GetWave(float2 uv) {
                float2 uvsCentred = uv * 2 - 1;
                float radialDistance = length(uvsCentred);
                float wave = cos((radialDistance - _Time.y * 0.1f) * TAU * 10);


                // float4 waves = t * (abs(i.normal.y) < 0.99);
                return wave *= 1 - radialDistance;
                //return abs(wave);
            }
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
                float2 uvsCentred = i.uv * 2 - 1;
                float radialDistance = length(uvsCentred);
                float xOffset = sin(cos(i.uv.y * TAU)) * 0.01f;
               // float t = cos((i.uv.y + xOffset + _Time.y) * TAU * 40) * 0.5 + 0.5;

                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _Color;

                float3 colOutput = lerp(_ColourA, _ColourB, i.uv.y) * col;
                float amount = sin(_Time.y *2) * 0.5 + 0.5;
                float3 color = lerp(float3(0, 0, 0), colOutput, _Transparency*1-(radialDistance-0.1f));
             
                return float4(color, 0);

                //return float4(i.normal,0);
                //return
            }
            ENDCG
        }
        }
}
