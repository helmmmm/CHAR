Shader "Voxel toolkit/Voxel"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert

        #pragma target 3.0

        struct vertexData
        {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;
            float4 texcoord2 : TEXCOORD2;
            float4 texcoord3 : TEXCOORD3;
            fixed4 color : COLOR;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };
        
        struct Input
        {
            float4 color : COLOR;
            float2 lightmapUV : TEXCOORD2;
            float4 parameters : TEXCOORD3;
        };

        void vert (inout vertexData v, out Input o)
        {
            o.color = v.color;
            o.parameters = v.texcoord3;
            o.lightmapUV = v.texcoord2;
        }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = IN.color;
            o.Alpha = IN.color.a;
            o.Smoothness = 1.0f - IN.parameters.x;
            o.Metallic = 1.0f - IN.parameters.y;
            o.Emission = IN.color.rgb * (IN.parameters.z * 10.0f + IN.parameters.w * 100.0f);
        }
        ENDCG
    }
}
