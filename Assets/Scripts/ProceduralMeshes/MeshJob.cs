using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Utilities.ProceduralMeshes
{
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct MeshJob<TMeshGenerator, TMeshStreams> 
        : IJobFor where TMeshGenerator : struct, IMeshGenerator 
                  where TMeshStreams : struct, IMeshStreams
    {
        [WriteOnly] private TMeshStreams   m_streams;
        private             TMeshGenerator m_generator;

        private int    Resolution  { get => m_generator.Resolution; set => m_generator.Resolution = value; }
        private int    VertexCount => m_generator.VertexCount;
        private int    IndexCount  => m_generator.IndexCount;
        private Bounds Bounds      => m_generator.Bounds;
        private int    JobLength   => m_generator.JobLength;

        public void Execute(int i) => m_generator.Execute(i, m_streams);

        public static JobHandle ScheduleParallel(Mesh mesh, Mesh.MeshData meshData, int resolution, JobHandle dependency)
        {
            var job = new MeshJob<TMeshGenerator, TMeshStreams>();
            job.Resolution = resolution;
            job.m_streams.Setup(meshData, mesh.bounds = job.Bounds, job.VertexCount, job.IndexCount);
            return job.ScheduleParallel(job.JobLength, 1, dependency);
        }
    }

    public delegate JobHandle MeshJobScheduleDelegate(Mesh mesh, Mesh.MeshData meshData, int resolution, JobHandle dependency);
}