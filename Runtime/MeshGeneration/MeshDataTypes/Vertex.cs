using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace MeshGeneration.MeshDataTypes
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public float3 position;
        public float3 normal;
        public float4 tangent;
        public float2 texCoords0;
        
        public float3 Position { set => position = value; }

        public float3 Normal {set => normal = value; }

        public float4 Tangent {set => tangent = value; }

        public float2 TexCoords0 { set => texCoords0 = value; }
    }
}
