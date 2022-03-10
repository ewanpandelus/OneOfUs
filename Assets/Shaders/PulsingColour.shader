﻿Shader "Unlit/PulsingColour"
{
    Properties
    {

        [HDR] _ColourA("Colour A", Color) = (0,1,0,1)
        _MainTex("Texture", 2D) = "white" {}


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
       
             sampler2D _MainTex;
     





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



                 fixed4 col = tex2D(_MainTex, i.uv);
                 col *= _ColourA;
          
                 float flashEffect = cos(_Time.y * 5) * 0.5;

                 return float4(col*(flashEffect+0.8));

                 
             }
             ENDCG
         }
         }
}