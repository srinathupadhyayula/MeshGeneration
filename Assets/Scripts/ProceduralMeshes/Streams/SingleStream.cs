using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Utilities.ProceduralMeshes.Streams
{
    public struct SingleStream : IMeshStreams
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Stream0
        {
            public float3 Position   { get; internal set; }
            public float3 Normal     { get; internal set; }
            public float4 Tangent    { get; internal set; }
            public float2 TexCoords0 { get; internal set; }
        };

        [NativeDisableContainerSafetyRestriction]
        private NativeArray<Stream0> m_stream0;
        [NativeDisableContainerSafetyRestriction]
        NativeArray<TriangleUInt16> m_triangles;
        
        public void Setup(Mesh.MeshData meshData, Bounds bounds, int vertexCount, int indexCount)
        {
            var descriptor = new NativeArray<VertexAttributeDescriptor>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            descriptor[0] = new VertexAttributeDescriptor(attribute: VertexAttribute.Position,  dimension: 3);
            descriptor[1] = new VertexAttributeDescriptor(attribute: VertexAttribute.Normal,    dimension: 3);
            descriptor[2] = new VertexAttributeDescriptor(attribute: VertexAttribute.Tangent,   dimension: 4);
            descriptor[3] = new VertexAttributeDescriptor(attribute: VertexAttribute.TexCoord0, dimension: 2);
            meshData.SetVertexBufferParams(vertexCount, descriptor);
            descriptor.Dispose();

            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt16);
			
            meshData.subMeshCount = 1;
            meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount) {bounds = bounds, vertexCount = vertexCount}
                              , MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontValidateIndices);
            
            m_stream0 = meshData.GetVertexData<Stream0>();
            m_triangles = meshData.GetIndexData<ushort>().Reinterpret<TriangleUInt16>(sizeof(ushort));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int index, Vertex vertexData) =>
            m_stream0[index] = new Stream0
                               {
                                   Position   = vertexData.position
                                 , Normal     = vertexData.normal
                                 , Tangent    = vertexData.tangent
                                 , TexCoords0 = vertexData.texCoords0
                               };

        public void SetTriangle(int index, int3 triangleData) => m_triangles[index] = triangleData;
    }
}