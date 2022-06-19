using UnityEngine;
using static Unity.Mathematics.math;
using float2 = Unity.Mathematics.float2;

namespace ProceduralMeshes.Generators
{
    public struct SquareGridMesh : IMeshGenerator
    {
        public int    Resolution  { get; set; }
        
        public int    VertexCount => 4 * Resolution * Resolution;
        public int    IndexCount  => 6 * Resolution * Resolution;
        public int    JobLength   => Resolution;
        public Bounds Bounds      => new Bounds(Vector3.zero, new Vector3(1f, 1f));

        private const float Offset   = 1f;
        private const int   ViOffset = 4;
        private const int   TiOffset = 2;
        public void Execute<TMeshStreams>(int y, TMeshStreams streams) 
            where TMeshStreams : struct, IMeshStreams
        {
            int vi = 4 * Resolution * y;
            int ti = 2 * Resolution * y;
            
            for (var x = 0; x < Resolution; ++x, vi += ViOffset, ti += TiOffset)
            {
                float2 xCoordinates = float2(x, x + Offset) / Resolution - 0.5f;
                float2 yCoordinates = float2(y, y + Offset) / Resolution - 0.5f;
                var    vertex       = new Vertex();
                
                vertex.normal.z   = -1f;
                vertex.tangent.xw = new float2(1f, -1f);

                vertex.position.x = xCoordinates.x;
                vertex.position.y = yCoordinates.x;
                streams.SetVertex(vi + 0, vertex);
			
                vertex.position.x = xCoordinates.y;
                vertex.texCoords0 = float2(1f, 0f);
                streams.SetVertex(vi + 1, vertex);

                vertex.position.x = xCoordinates.x;
                vertex.position.y = yCoordinates.y;
                vertex.texCoords0 = float2(0f, 1f);
                streams.SetVertex(vi + 2, vertex);

                vertex.position.x = xCoordinates.y;
                vertex.texCoords0 = float2(1f, 1f);
                streams.SetVertex(vi + 3, vertex);
            
                streams.SetTriangle(ti + 0, vi + int3(0, 2, 1));
                streams.SetTriangle(ti + 1, vi + int3(2, 3, 1));
            }
        }
    }
}