using UnityEngine;

namespace MeshGeneration.AbstractInterfaces
{
    public interface IMeshGenerator
    {
        public void Generate(ref Mesh mesh);

        public void SetupMeshStreams(ref Mesh.MeshData meshData);
        
        public void CalculateVertices();
        
        public void CalculateTriangles();

        public void CompleteJobs();
        
        public void UpdateMeshStreams();
        
        public void DisposeAll();
    }
}