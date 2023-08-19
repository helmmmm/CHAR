using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

namespace VoxelToolkit
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public readonly float3 Position;
        public readonly byte NormalX, NormalY, NormalZ, NormalW;
        public readonly Color32 Color;
        public readonly int Parameters;

        public Vertex(float3 position, int3 normal, TransformedMaterial material)
        {
            Position = position;
            NormalX = (byte)normal.x;
            NormalY = (byte)normal.y;
            NormalZ = (byte)normal.z;
            NormalW = 0;

            Color = material.Color;
            Parameters = material.Parameters;
        }
    }
}