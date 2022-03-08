Shader "Unlit/InfluenceBar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Influence("Influence", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}


        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha 
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            float _Influence;

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float _Health;
            float InverseLerp(float a, float b, float v) {
                return (v - a) / (b - a);

            }
            Interpolators vert (MeshData v)
            {
                Interpolators o;
                UNITY_INITIALIZE_OUTPUT(Interpolators, o);
                o.uv = v.uv;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            
            float4 frag(Interpolators i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //return col;
                float xOffset = i.uv.x/4;
        
                
                float flashEffect = cos(_Time.y * 4) * 0.2;
              
               
             
                float influenceBarMask = i.uv.x < _Influence;
  
              // clip(influenceBarMask - 0.5);
                float tInfluenceColour = saturate(InverseLerp(0.2,0.8,_Influence));
                float3 tInfluenceBarColour = lerp(lerp(float3(1, 0, 0), float3(0,0,1), tInfluenceColour), lerp(float3(0, 0, 1), float3(0, 1, 0), tInfluenceColour -0.2), tInfluenceColour);
          

                return float4(tInfluenceBarColour*(flashEffect+1), influenceBarMask *0.9);
              

            }
            ENDCG
        }
    }
}
