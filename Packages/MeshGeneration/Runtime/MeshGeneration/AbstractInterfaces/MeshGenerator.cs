using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MeshGeneration.MeshDataTypes;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Mathematics;

namespace MeshGeneration.AbstractInterfaces
{
    [System.Serializable]
    public abstract class MeshGeneratorBase : ScriptableObject, IMeshGenerator
    {
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<Vertex> m_vertexStream;

        [NativeDisableContainerSafetyRestriction]
        private NativeArray<TriangleUInt16> m_triangleStream;

        protected abstract NativeArray<Vertex> Vertices    { get; set; }
        protected abstract NativeArray<int3>   Triangles   { get; set; }
        protected abstract int                 VertexCount { get; }
        protected abstract int                 IndexCount  { get; }


        public void Generate([NotNull] ref Mesh mesh)
        {
            if (mesh == null)
                throw new ArgumentNullException(nameof(mesh));

            Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData      meshData      = meshDataArray[0];

            SetupMeshStreams(ref meshData);
            CalculateVertices();
            CalculateTriangles();
            CompleteJobs();
            UpdateMeshStreams();
            DisposeAll();

            mesh.RecalculateBounds();
            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        }

        public void SetupMeshStreams(ref Mesh.MeshData meshData)
        {
            var descriptor = new NativeArray<VertexAttributeDescriptor>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

            descriptor[0] = new VertexAttributeDescriptor(attribute: VertexAttribute.Position,  dimension: 3);
            descriptor[1] = new VertexAttributeDescriptor(attribute: VertexAttribute.Normal,    dimension: 3);
            descriptor[2] = new VertexAttributeDescriptor(attribute: VertexAttribute.Tangent,   dimension: 4);
            descriptor[3] = new VertexAttributeDescriptor(attribute: VertexAttribute.TexCoord0, dimension: 2);

            meshData.SetVertexBufferParams(VertexCount, descriptor);
            meshData.SetIndexBufferParams(IndexCount, IndexFormat.UInt16);
            descriptor.Dispose();

            meshData.subMeshCount = 1;
            meshData.SetSubMesh(0, new SubMeshDescriptor(0, IndexCount)
                                   {
                                       /*bounds = Bounds,*/ vertexCount = VertexCount
                                   }
                              , MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontValidateIndices);

            m_vertexStream   = meshData.GetVertexData<Vertex>();
            m_triangleStream = meshData.GetIndexData<ushort>().Reinterpret<TriangleUInt16>(sizeof(ushort));
        }

        #region Abstract Methods

        public abstract void CalculateVertices();
        public abstract void CalculateTriangles();
        public abstract void CompleteJobs();

        #endregion // Abstract Methods

        public void UpdateMeshStreams()
        {
            for (var vi = 0; vi < m_vertexStream.Length; ++vi)
            {
                SetVertex(vi, Vertices[vi]);
            }

            for (var index = 0; index < m_triangleStream.Length; index++)
            {
                SetTriangle(index, Triangles[index]);
            }
        }

        public void DisposeAll()
        {
            if (m_vertexStream.IsCreated)
            {
                m_vertexStream.Dispose();
            }

            if (m_triangleStream.IsCreated)
            {
                m_triangleStream.Dispose();
            }

            if (Vertices.IsCreated)
            {
                Vertices.Dispose();
            }

            if (Triangles.IsCreated)
            {
                Triangles.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int index, Vertex vertexData) =>
            m_vertexStream[index] = new Vertex
                                    {
                                        Position   = vertexData.position
                                      , Normal     = vertexData.normal
                                      , Tangent    = vertexData.tangent
                                      , TexCoords0 = vertexData.texCoords0
                                    };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTriangle(int index, int3 triangleData) => m_triangleStream[index] = triangleData;
    }
}