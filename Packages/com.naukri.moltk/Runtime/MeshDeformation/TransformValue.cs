using UnityEngine;

namespace Naukri.Moltk.MeshDeformation
{
    internal struct TransformValue
    {
        internal TransformValue(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            localScale = transform.localScale;
        }

        public Vector3 position;

        public Quaternion rotation;

        public Vector3 localScale;
    }
}
