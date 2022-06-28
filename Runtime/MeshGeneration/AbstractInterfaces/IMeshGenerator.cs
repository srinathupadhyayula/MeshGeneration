using MeshGeneration.MeshDataTypes;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MeshGeneration.AbstractInterfaces
{
    public interface IMeshGenerator
    {
        public NativeArray<Vertex> Vertices           { get; set; }
        public NativeArray<int3>   Triangles          { get; set; }
        public JobHandle           VerticesJobHandle  { get; set; }
        public JobHandle           TrianglesJobHandle { get; set; }
        public IJobFor             VerticesJob        { get; set; }
        public IJobFor             TrianglesJob       { get; set; }

        public    void                Generate(ref Mesh mesh);

        public void SetupMeshStreams(ref Mesh.MeshData meshData);
        
        public void CalculateVertices();
        
        public void CalculateTriangles();

        public void FinishMeshCalculations();
        
        public void UpdateMeshStreams();
        
        public void DisposeAll();
    }
}