﻿using Unity.Mathematics;
using UnityEngine;

namespace Utilities.ProceduralMeshes
{
    public interface IMeshStreams
    {
        void Setup(Mesh.MeshData meshData, Bounds bounds, int vertexCount, int indexCount);
        void SetVertex(int       index,    Vertex vertexData);
        void SetTriangle(int     index,    int3   triangleData);
    }
}