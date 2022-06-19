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

        
        public void Execute<TMeshStreams>(int y, TMeshStreams streams) 
            where TMeshStreams : struct, IMeshStreams
        {
            return;
            int vi     = (Resolution + 1) * y;
            int ti     = (2 * Resolution) * (y - 1);

            var vertex = new Vertex();
            vertex.normal.z   = 1f;
            vertex.tangent.xw = float2(1f, -1f);
            
            vertex.position.x   = -0.5f;
            vertex.position.y   = (float)y / Resolution - 0.5f;
            vertex.texCoords0.y = (float)y / Resolution;
            streams.SetVertex(vi, vertex);
            
            ++vi;

            for (var x = 1; x <= Resolution; ++x, ++vi, ti+=2)
            {
                vertex.position.x  = (float)x / Resolution - 0.5f;
                vertex.texCoords0.x = (float)x / Resolution;
                streams.SetVertex(vi, vertex);
                
                if (y > 0)
                {
                    Debug.LogWarning("MMMM123MMMM!");
                    streams.SetTriangle(ti + 0, vi + int3(-Resolution - 2, -1, -Resolution - 1));
                    streams.SetTriangle(ti + 1, vi + int3(-Resolution - 1, -1, 0));
                }
            }
        }
    }
}