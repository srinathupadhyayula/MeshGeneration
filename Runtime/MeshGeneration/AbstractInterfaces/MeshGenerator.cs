using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MeshGeneration.MeshDataTypes;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Mathematics;

namespace MeshGeneration.AbstractInterfaces
{
    [Serializable]
    public abstract class MeshGenerator<TMeshData> : MeshGeneratorBase
    {
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<Vertex> m_vertexStream;

        [NativeDisableContainerSafetyRestriction]
        private NativeArray<TriangleUInt16> m_triangleStream;
        
        private            NativeArray<Vertex> m_vertices;
        private            NativeArray<int3>   m_triangles;
        private            JobHandle           m_verticesHandle;
        private            JobHandle           m_trianglesJobHandle;
       

        protected override NativeArray<Vertex> Vertices           { get => m_vertices;           set => m_vertices = value; }
        protected override NativeArray<int3>   Triangles          { get => m_triangles;          set => m_triangles = value; }
        protected override JobHandle           VerticesJobHandle  { get => m_verticesHandle;     set => m_verticesHandle = value; }
        protected override JobHandle           TrianglesJobHandle { get => m_trianglesJobHandle; set => m_trianglesJobHandle = value; }
        
        [field: SerializeField] public TMeshData Data                              { get; set; }
        [field: SerializeField] public bool      RecalculateBounds                 { get; set; }
        [field: SerializeField] public bool      OptimizeGeneratedMesh             { get; set; }
        [field: SerializeField] public bool      RecalculateUVDistributionsMetrics { get; set; }
        [field: SerializeField] public bool      RecalculateNormals                { get; set; }
        [field: SerializeField] public bool      RecalculateTangents               { get; set; }
        

        public override void Generate([NotNull] ref Mesh mesh)
        {
            if (mesh == null)
            {
                mesh = new Mesh();
            }

            Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData      meshData      = meshDataArray[0];

            SetupMeshStreams(ref meshData);
            CalculateVertices();
            CalculateTriangles();
            FinishMeshCalculations();
            UpdateMeshStreams();
            DisposeAll();

            if (RecalculateUVDistributionsMetrics)
            {
                mesh.RecalculateUVDistributionMetrics();
            }
            
            if (RecalculateNormals)
            {
                mesh.RecalculateNormals();
            }
            
            
            if (RecalculateTangents)
            {
                mesh.RecalculateTangents();
            }
            
            if (OptimizeGeneratedMesh)
            {
                mesh.OptimizeReorderVertexBuffer();
                mesh.OptimizeIndexBuffers();
                mesh.Optimize();
            }

            if (RecalculateBounds)
            {
                mesh.RecalculateBounds();
            }
            
            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        }

        protected override void SetupMeshStreams(ref Mesh.MeshData meshData)
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

        protected abstract override void CalculateVertices();
        protected abstract override void CalculateTriangles();
        protected abstract override void FinishMeshCalculations();

        #endregion // Abstract Methods

        protected override void UpdateMeshStreams()
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

        protected override void DisposeAll()
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
        private void SetVertex(int index, Vertex vertexData) =>
            m_vertexStream[index] = new Vertex
                                    {
                                        Position   = vertexData.position
                                      , Normal     = vertexData.normal
                                      , Tangent    = vertexData.tangent
                                      , TexCoords0 = vertexData.texCoords0
                                    };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetTriangle(int index, int3 triangleData) => m_triangleStream[index] = triangleData;
    }
}