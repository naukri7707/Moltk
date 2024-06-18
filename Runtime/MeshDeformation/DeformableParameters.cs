using UnityEngine;

namespace Naukri.Moltk.MeshDeformation
{
    [RequireComponent(typeof(DeformableObject))]
    public abstract class DeformableParameters : MonoBehaviour
    {
        protected virtual void Reset()
        {
            var deformableObject = GetComponent<DeformableObject>();
            deformableObject.parameters = this;
        }
    }
}