using MeshGeneration.MeshDataTypes;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MeshGeneration.AbstractInterfaces
{
    public abstract class MeshGeneratorBase : ScriptableObject
    {
        public abstract NativeArray<Vertex> Vertices           { get; set; }
        public abstract NativeArray<int3>   Triangles          { get; set; }
        public abstract JobHandle           VerticesJobHandle  { get; set; }
        public abstract JobHandle           TrianglesJobHandle { get; set; }
        public abstract IJobFor             VerticesJob        { get; set; }
        public abstract IJobFor             TrianglesJob       { get; set; }
        public abstract void                Generate(ref         Mesh          mesh);
        public abstract void                SetupMeshStreams(ref Mesh.MeshData meshData);
        public abstract void                CalculateVertices();
        public abstract void                CalculateTriangles();
        public abstract void                FinishMeshCalculations();
        public abstract void                UpdateMeshStreams();
        public abstract void                DisposeAll();
    }
}