using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace Utilities.ProceduralMeshes
{
    public struct Vertex
    {
        public float3 position;
        public float3 normal;
        public float4 tangent;
        public float2 texCoords0;
    }
}
