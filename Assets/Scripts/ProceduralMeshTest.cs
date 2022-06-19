using ProceduralMeshes;
using ProceduralMeshes.Generators;
using ProceduralMeshes.Streams;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralMeshTest : MonoBehaviour
{
    [field: SerializeField] private MeshFilter   MeshFilterComponent          { get; set; }
    [field: SerializeField] private MeshRenderer MeshRendererComponent        { get; set; }
    [field: SerializeField] private Material     MaterialToUseForMeshRenderer { get; set; }

    [field: Range(1, 10)]
    [field: SerializeField]
    private int Resolution { get; set; } = 1;
        
    private static readonly MeshJobScheduleDelegate[] Jobs =
    {
        MeshJob<SquareGridMesh, SingleStream>.ScheduleParallel
      , MeshJob<SquareGridMesh, MultiStream>.ScheduleParallel
      , MeshJob<SharedSquareGridMesh, SingleStream>.ScheduleParallel
      , MeshJob<SharedSquareGridMesh, MultiStream>.ScheduleParallel
    };

    public enum MeshType
    {
        SquareGridSingleStream
      , SquareGridMultiStream
      , SharedSquareGridSingleStream
      , SharedSquareGridMultiStream
    };

    [SerializeField] private MeshType m_meshType;

    private Mesh m_mesh;

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        GenerateMesh();
        enabled = false;
    }

    private void OnValidate()
    {
        enabled = true;
        CacheComponents();
        InitializeMeshRenderer();
    }

    private void Initialize()
    {
        CacheComponents();
        InitializeMeshRenderer();
        CreateMesh();
    }

    private void CacheComponents()
    {

        MeshFilterComponent   = MeshFilterComponent ? MeshFilterComponent : GetComponent<MeshFilter>();
        MeshRendererComponent = MeshRendererComponent ? MeshRendererComponent : GetComponent<MeshRenderer>();
    }
        
    private void InitializeMeshRenderer()
    {
        if (MeshRendererComponent)
        {
            MeshRendererComponent.sharedMaterial = MaterialToUseForMeshRenderer;
        }
    }

    private void CreateMesh()
    {
        m_mesh = new Mesh {name = "Procedural Mesh"};
    }

    private void GenerateMesh()
    {
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData      meshData      = meshDataArray[0];

        Jobs[(int)m_meshType](m_mesh, meshData, Resolution, default).Complete();
        //MeshJob<SharedSquareGridMesh, MultiStream>.ScheduleParallel(m_mesh, meshData, Resolution, default).Complete();
        
        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, m_mesh);
        MeshFilterComponent.mesh = m_mesh;
    }
}