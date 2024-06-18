using UnityEngine;

namespace Naukri.Moltk.MeshDeformation
{
    internal struct TransformValue
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;

        internal TransformValue(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            localScale = transform.localScale;
        }
    }
}