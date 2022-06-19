using UnityEngine;
using static Unity.Mathematics.math;

namespace ProceduralMeshes.Generators
{
    public struct SharedSquareGridMesh : IMeshGenerator
    {
        public int    Resolution  { get; set; }
        
        public int    VertexCount => (Resolution + 1) * (Resolution + 1);
        public int    IndexCount  => 6 * Resolution * Resolution;
        public int    JobLength   => Resolution + 1;
        public Bounds Bounds      => new Bounds(Vector3.zero, new Vector3(1f, 1f));

        private const float Offset   = 1f;
        private const int   ViOffset = 4;
        private const int   TiOffset = 2;
        
        public void Execute<TMeshStreams>(int y, TMeshStreams streams) 
            where TMeshStreams : struct, IMeshStreams
        {
            int vi     = (Resolution + 1) * y;
            int ti     = (2 * Resolution) * (y - 1);
            var vertex = new Vertex();

            vertex.normal.y   = 1f;
            vertex.tangent.xw = float2(1f, -1f);
            
            vertex.position.x  = -0.5f;
            vertex.position.y  = (float)y / Resolution - 0.5f;
            vertex.texCoords0.y = (float)y / Resolution;
            streams.SetVertex(vi, vertex);
            
            vi += 1;

            for (var x = 1; x <= Resolution; x++, vi++)
            {
                vertex.position.x  = (float)x / Resolution - 0.5f;
                vertex.texCoords0.x = (float)x / Resolution;
                streams.SetVertex(vi, vertex);
            }
        }
    }
}