using UnityEngine;

namespace Naukri.Moltk.MeshBuilder
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public abstract class MeshBuilderBase : MonoBehaviour
    {
        [SerializeField]
        protected string meshName;

        private MeshFilter meshFilter;

        private MeshRenderer meshRenderer;

        public MeshFilter MeshFilter
        {
            get
            {
                if (meshFilter == null)
                {
                    meshFilter = GetComponent<MeshFilter>();
                }
                return meshFilter;
            }
        }

        public MeshRenderer MeshRenderer
        {
            get
            {
                if (meshRenderer == null)
                {
                    meshRenderer = GetComponent<MeshRenderer>();
                }
                return meshRenderer;
            }
        }

        public void BuildMesh()
        {
            var mesh = new Mesh()
            {
                name = meshName
            };

            OnMeshBuilding(mesh);
            MeshFilter.mesh = mesh;
        }

        public void DestoryMesh()
        {
            MeshFilter.sharedMesh = null;
        }

        public void RebuildMesh()
        {
            DestoryMesh();
            BuildMesh();
        }

        protected abstract void OnMeshBuilding(Mesh mesh);
    }
}
