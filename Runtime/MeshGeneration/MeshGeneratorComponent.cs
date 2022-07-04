using MeshGeneration.AbstractInterfaces;
using UnityEngine;

namespace MeshGeneration
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshGeneratorComponent : MonoBehaviour
    {
        [field: SerializeField] public MeshFilter        MeshFilterComponent          { get; private set; }
        [field: SerializeField] public MeshRenderer      MeshRendererComponent        { get; private set; }
        [field: SerializeField] public Material          MaterialToUseForMeshRenderer { get; private set; }
        [field: SerializeField] public string            MeshName                     { get; private set; } = "GeneratedMesh";
        [field: SerializeField] public MeshGeneratorBase Generator                    { get; private set; }

        private Mesh m_mesh;
        public bool m_regenMesh;
        
        private void Awake()
        {
            Initialize();
            Generate();
        }

        private void OnValidate()
        {
            if (!m_regenMesh)
            {
                return;
            }
            
            m_regenMesh = false;
            Initialize();
            Generate();
        }

        private void Initialize()
        {
            CacheComponents();
            CreateMesh();
            UpdateMeshFilter();
            InitializeMeshRenderer();
        }
        
        private void CacheComponents()
        {
            MeshFilterComponent   = MeshFilterComponent ? MeshFilterComponent : GetComponent<MeshFilter>();
            MeshRendererComponent = MeshRendererComponent ? MeshRendererComponent : GetComponent<MeshRenderer>();
        }
        
        private void CreateMesh()
        {
            if (m_mesh)
            {
                m_mesh.Clear();
            }
            else
            {
                m_mesh = new Mesh();
            }

            m_mesh.name = MeshName;
        }
        
        
        private void UpdateMeshFilter()
        {
            MeshFilterComponent.sharedMesh = m_mesh;
        }
        
        private void InitializeMeshRenderer()
        {
            if (MeshRendererComponent)
            {
                MeshRendererComponent.sharedMaterial = MaterialToUseForMeshRenderer;
            }
        }

        public void Generate()
        {
            if (Generator == null)
            {
                return;
            }
            
            Generator.Generate(ref m_mesh);
            UpdateMeshFilter();
        }
    }
}