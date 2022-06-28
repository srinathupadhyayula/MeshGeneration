using System;
using MeshGeneration.AbstractInterfaces;
using MeshGeneration.MeshDataTypes;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace MeshGeneration.Generators
{
    public struct CalculateVerticesJob : IJobFor
    {
        [NativeDisableParallelForRestriction] public NativeArray<Vertex> vertices;
        [ReadOnly]                            public int2                resolution;
        [ReadOnly]                            public int                 size;
        
        private int Columns              => resolution.x;
        private int Rows                 => resolution.y;
        private int VerticesInEachColumn => Columns + 1;

        public void Execute(int row)
        {
            int vi = VerticesInEachColumn * row;

            var vertex = new Vertex();
            vertex.normal.z   = 1f;
            vertex.tangent.xw = float2(1f, -1f);

            vertex.position.y   = row         * size;
            vertex.texCoords0.y = (float) row / Rows;

            vertices[vi] = vertex;

            ++vi;

            for (var column = 1; column < VerticesInEachColumn; ++column, ++vi)
            {
                vertex.position.x   = column         * size;
                vertex.texCoords0.x = (float) column / Columns;
                vertices[vi]        = vertex;
            }
        }
    }

    public struct CalculateTrianglesJob : IJobFor
    {
        [NativeDisableParallelForRestriction] public NativeArray<int3> triangles;

        [ReadOnly] public int2 resolution;

        private int Columns              => resolution.x;
        private int VerticesInEachColumn => Columns + 1;

        public void Execute(int row)
        {
            for (var column = 0; column < Columns; ++column)
            {
                int bottomLeft  = column     + VerticesInEachColumn * row;
                int topLeft     = bottomLeft + VerticesInEachColumn;
                int bottomRight = bottomLeft + 1;
                int topRight    = topLeft    + 1;

                int triangleAIndex = 2 * Columns * row + 2 * column;
                int triangleBIndex = triangleAIndex    + 1;

                int3 triangleA = int3(bottomLeft,  topLeft, bottomRight);
                int3 triangleB = int3(bottomRight, topLeft, topRight);

                //Debug.Log($"\nti:{ti}\nresolution.x:{resolution.x}\nrow:{row}\ncolumn:{column}");
                triangles[triangleAIndex] = triangleA;
                triangles[triangleBIndex] = triangleB;
            }
        }
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/MeshGeneration/Generators/SharedSquareGridGenerator"
                   , fileName = "SharedSquareGridGenerator")]
    public class SharedSquareGridGenerator : MeshGenerator
    {
        [Serializable]
        public struct GridResolution
        {
            [Min(1)] public int x;
            [Min(1)] public int y;
        }

        [field: SerializeField] public GridResolution Resolution    { get; set; }
        int                                           NumCells      => Resolution.x       * Resolution.y;
        private            int                        TriangleCount => NumCells           * 2;
        protected override int                        VertexCount   => (Resolution.x + 1) * (Resolution.y + 1);
        protected override int                        IndexCount    => 6                  * NumCells;

        public override void CalculateVertices()
        {
            Vertices          = new NativeArray<Vertex>(VertexCount, Allocator.TempJob);
            VerticesJob       = new CalculateVerticesJob {vertices = Vertices, size = 1, resolution = int2(Resolution.x, Resolution.y)};
            VerticesJobHandle = ((CalculateVerticesJob) VerticesJob).ScheduleParallel(Resolution.y + 1, 1, default);
        }

        public override void CalculateTriangles()
        {
            Triangles          = new NativeArray<int3>(TriangleCount, Allocator.TempJob);
            TrianglesJob       = new CalculateTrianglesJob {triangles = Triangles, resolution = int2(Resolution.x, Resolution.y)};
            TrianglesJobHandle = ((CalculateTrianglesJob) TrianglesJob).ScheduleParallel(Resolution.y, 1, default);
        }
        
        public override void FinishMeshCalculations()
        {
            VerticesJobHandle.Complete();
            TrianglesJobHandle.Complete();

            Vertices.CopyFrom(((CalculateVerticesJob) VerticesJob).vertices);
            Triangles.CopyFrom(((CalculateTrianglesJob) TrianglesJob).triangles);
        }
    }
}