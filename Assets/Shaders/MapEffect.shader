Shader "Unlit/MapEffect"
{
    Properties
    {

        [HDR] _ColourA("Colour A", Color) = (0,1,0,1)
        _MainTex("MainTex", 2D) = "white" {}
        

    }
        SubShader
        {
            Tags { "RenderType" = "Opaque"}
            LOD 100

            Pass
            {
                ZWrite Off
                Cull Off
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
            //#pragma surface surf Standard fullforwardshadows vertex:vert

            #define TAU 6.283185
            #include "UnityCG.cginc"


 
            float4 _MainTex_ST;
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
                float4 texColor;
                fixed4 col = tex2D(_MainTex, i.uv);
                float average = (col.x + col.y + col.z) / 3.0;

                // Check if it's closer to white or black
                texColor = float4(average, average, average, 1);
                return texColor*float4(0.74, 0.72549, 0.55, 1);
            }
            ENDCG
        }
        }
}
