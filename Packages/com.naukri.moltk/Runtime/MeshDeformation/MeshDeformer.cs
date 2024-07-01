using UnityEngine;

namespace Naukri.Moltk.MeshDeformation
{
    public abstract class MeshDeformer : MonoBehaviour
    {
        public static float modifyDistanceThreshold = 0.001F;

        public void Deform(DeformableObject deformable)
        {
            OnDeform(deformable);
        }

        protected virtual void Awake()
        {
        }

        protected abstract void OnDeform(DeformableObject deformable);
    }
}
