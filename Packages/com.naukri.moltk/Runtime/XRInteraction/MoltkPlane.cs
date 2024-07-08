using UnityEngine;

namespace Naukri.Moltk.XRInteraction
{
    public class MoltkPlane : MonoBehaviour
    {
        [SerializeField]
        private bool showPlane;

        public static MoltkPlane Create(Transform target)
        {
            var plane = new GameObject($"[{target.name}] Plane");
            plane.transform.SetParent(target.parent);
            plane.transform.SetLocalPositionAndRotation(target.localPosition, target.localRotation);
            plane.transform.SetSiblingIndex(target.GetSiblingIndex() + 1);
            return plane.AddComponent<MoltkPlane>();
        }

        public float CalcSignedAngle(Transform transform)
        {
            return CalcSignedAngle(transform.position);
        }

        public float CalcSignedAngle(Vector3 position)
        {
            var direction = position - transform.position;
            var planeNormal = transform.forward;
            planeNormal.Normalize();
            var projectedPoint = Vector3.ProjectOnPlane(direction, planeNormal);
            var projectedVector = projectedPoint.normalized;

            var angle = Vector3.SignedAngle(transform.up, projectedVector, planeNormal);
            return angle;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            DrawGizmos();
        }

        protected virtual void OnDrawGizmos()
        {
            if (showPlane)
            {
                DrawGizmos();
            }
        }

        private void DrawGizmos()
        {
            if (!showPlane)
            {
                return;
            }

            // 計算投影平面的法線
            var planeNormal = transform.forward;
            planeNormal.Normalize();

#if UNITY_EDITOR
            // Draw the plane
            UnityEditor.Handles.color = Color.green;
            for (var i = 1; i <= 10; i++)
            {
                UnityEditor.Handles.DrawWireDisc(transform.position, planeNormal, i * 0.05F);
            }
#endif

            // Draw the direction of the plane
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);

            // Draw the zero angle axis
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.up);
        }
    }
}
