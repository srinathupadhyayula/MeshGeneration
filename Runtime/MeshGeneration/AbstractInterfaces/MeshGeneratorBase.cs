using MeshGeneration.MeshDataTypes;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MeshGeneration.AbstractInterfaces
{
    public abstract class MeshGeneratorBase : ScriptableObject
    {
        protected abstract NativeArray<Vertex> Vertices           { get; set; }
        protected abstract NativeArray<int3>   Triangles          { get; set; }
        protected abstract JobHandle           VerticesJobHandle  { get; set; }
        protected abstract JobHandle           TrianglesJobHandle { get; set; }
        protected abstract IJobFor             VerticesJob        { get; set; }
        protected abstract IJobFor             TrianglesJob       { get; set; }

        protected abstract int VertexCount   { get; }
        protected abstract int IndexCount    { get; }
        protected abstract int TriangleCount { get; }

        protected abstract void SetupMeshStreams(ref Mesh.MeshData meshData);
        protected abstract void CalculateVertices();
        protected abstract void CalculateTriangles();
        protected abstract void FinishMeshCalculations();
        protected abstract void UpdateMeshStreams();
        protected abstract void DisposeAll();

        public abstract void Generate(ref Mesh mesh);
    }
}