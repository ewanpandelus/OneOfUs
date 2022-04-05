Shader "Unlit/FBM"
{
    Properties
    {

          [HDR] _ColourA("Colour A", Color) = (0,1,0,1)
          [HDR] _ColourB("Colour B", Color) = (0,1,0,1)

        _MainTex("Texture", 2D) = "white" {}
   
    }
        SubShader
         {
             Tags { "RenderType" = "Trasparent" "Queue" = "Transparent"}
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


        
             float4 _ColourA;
             float4 _ColourB;
         
             float4 _MainTex_ST;
             sampler2D _MainTex;
          

             float hash(float2 n)
             {
                 return frac(sin(dot(n, float2(123.456789, 987.654321))) * 54321.9876);
             }

             float noise(float2 p)
             {
                 float2 i = floor(p);
                 float2 u = smoothstep(0.0, 1.0, frac(p));
                 float a = hash(i + float2(0, 0));
                 float b = hash(i + float2(1, 0));
                 float c = hash(i + float2(0, 1));
                 float d = hash(i + float2(1, 1));
                 float r = lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
                 return r * r;
             }

             float fbm(float2 p, int octaves)
             {
                 float value = 0.0;
                 float amplitude = 0.5;
                 float e = 3.0;
                 for (int i = 0; i < octaves; ++i)
                 {
                     value += amplitude * noise(p);
                     p = p * e;
                     amplitude *= 0.5;
                     e *= 0.95;
                 }
                 return value;
             }


             float GetWave(float2 uv) {
                 float2 uvsCentred = uv * 2 - 1;
                 float radialDistance = length(uvsCentred);
                 float wave = cos((radialDistance - _Time.y * 0.01f) * TAU * 10);


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
                float xOffset = sin(cos(i.uv.x * TAU * 8)) * 0.05f;
         
                fixed4 col = tex2D(_MainTex, i.uv);
              
                float4 crazy =  cos((radialDistance * lerp(_ColourA, _ColourB, radialDistance) - _Time.y * 0.1f) * TAU * 10) * col;
                
                float f = fbm(i.uv + _Time.y+ fbm(5 * i.uv + _Time.y, 20), 20);
                
                float3 fbmColor = lerp(_ColourA, _ColourB, 2 * f);
                
                float4 output = float4(fbmColor, 1);
            
                return output;

                 //return float4(i.normal,0);
                 //return
             }
             ENDCG
         }
         }
}
