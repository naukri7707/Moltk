using Naukri.InspectorMaid;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Naukri.Moltk.Outline.Tooltip
{
    [RequireComponent(typeof(LineRenderer))]
    public class TooltipLine : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private Tooltip _tooltip;

        [SerializeField, ReadOnly]
        private LineRenderer _lineRenderer;

        [
            RowScope, HideIf(binding: nameof(ReferenceExists)),
                HelpBox("Missing reference.", HelpBoxMessageType.Error), Style(flexGrow: "1"),
                Button("Fix References", binding: nameof(FixReference), setDirty: true),
            EndScope,
            Target, Hide
        ]
        private bool ReferenceExists => _tooltip != null && _lineRenderer != null;

        private void FixReference()
        {
            _tooltip = GetComponentInParent<Tooltip>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.positionCount = 3;
        }

        private void Update()
        {
            var localScaleYOnTooltip = _tooltip.Text.transform.lossyScale.y / transform.parent.lossyScale.y;
            var localPositionOnTooltip = transform.parent.InverseTransformPoint(_tooltip.Text.transform.position);

            var endPos = localPositionOnTooltip
                         - new Vector3(0, _tooltip.Text.preferredHeight * 0.5F * localScaleYOnTooltip, 0);

            var vertexPos = Vector3Multi(endPos, _tooltip.InflectionPointRatio);
            _lineRenderer.SetPosition(1, vertexPos);
            _lineRenderer.SetPosition(2, endPos);
        }

        public void SetColor(Color color)
        {
            _lineRenderer.startColor = _lineRenderer.endColor = color;
        }

        public void SetWidth(float width)
        {
            _lineRenderer.startWidth = _lineRenderer.endWidth = width;
        }

        private static Vector3 Vector3Multi(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3 { x = lhs.x * rhs.x, y = lhs.y * rhs.y, z = lhs.z * rhs.z };
        }
    }
}