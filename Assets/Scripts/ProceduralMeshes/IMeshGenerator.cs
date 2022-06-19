using UnityEngine;

namespace ProceduralMeshes
{
    public interface IMeshGenerator
    {
        int    VertexCount { get; }
        int    IndexCount  { get; }
        Bounds Bounds      { get; }
        int    JobLength   { get; }
        
        int Resolution { get; set; }
        
        void Execute<TMeshStreams>(int index, TMeshStreams streams) where TMeshStreams : struct, IMeshStreams;
    }
}