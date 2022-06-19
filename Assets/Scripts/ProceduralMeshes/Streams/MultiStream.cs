using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Utilities.ProceduralMeshes.Streams
{
    public struct MultiStream : IMeshStreams
    {
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<float3> m_stream0;

        [NativeDisableContainerSafetyRestriction]
        private NativeArray<float3> m_stream1;

        [NativeDisableContainerSafetyRestriction]
        private NativeArray<float4> m_stream2;

        [NativeDisableContainerSafetyRestriction]
        private NativeArray<float2> m_stream3;
        
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<TriangleUInt16> m_triangles;
        
        public void Setup(Mesh.MeshData meshData, Bounds bounds, int vertexCount, int indexCount)
        {
            var descriptor = new NativeArray<VertexAttributeDescriptor>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            descriptor[0] = new VertexAttributeDescriptor(attribute: VertexAttribute.Position,  dimension: 3, stream: 0);
            descriptor[1] = new VertexAttributeDescriptor(attribute: VertexAttribute.Normal,    dimension: 3, stream: 1);
            descriptor[2] = new VertexAttributeDescriptor(attribute: VertexAttribute.Tangent,   dimension: 4, stream: 2);
            descriptor[3] = new VertexAttributeDescriptor(attribute: VertexAttribute.TexCoord0, dimension: 2, stream: 3);
            meshData.SetVertexBufferParams(vertexCount, descriptor);
            descriptor.Dispose();

            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt16);
			
            meshData.subMeshCount = 1;
            meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount) {bounds = bounds, vertexCount = vertexCount}
                              , MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontValidateIndices);
            
            m_stream0   = meshData.GetVertexData<float3>();
            m_stream1   = meshData.GetVertexData<float3>(1);
            m_stream2   = meshData.GetVertexData<float4>(2);
            m_stream3   = meshData.GetVertexData<float2>(3);
            m_triangles = meshData.GetIndexData<ushort>().Reinterpret<TriangleUInt16>(sizeof(ushort));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int index, Vertex vertexData)
        {
            m_stream0[index] = vertexData.position;
            m_stream1[index] = vertexData.normal;
            m_stream2[index] = vertexData.tangent; 
            m_stream3[index] = vertexData.texCoords0;
        }

        public void SetTriangle(int index, int3 triangleData) => m_triangles[index] = triangleData;
    }
}