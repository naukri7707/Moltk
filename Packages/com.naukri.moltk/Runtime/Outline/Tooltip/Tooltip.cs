using Naukri.InspectorMaid;
using UnityEngine;
using UnityEngine.UI;

namespace Naukri.Moltk.Outline.Tooltip
{
    public class Tooltip : MonoBehaviour
    {
        [field: SerializeField, ReadOnly]
        public TooltipLine TooltipLine { get; set; }

        [field: SerializeField]
        public Transform Anchor { get; set; }

        [field: SerializeField]
        public Transform Face { get; set; }

        [field: SerializeField]
        public Text Text { get; set; }

        [field: SerializeField]
        public Vector3 InflectionPointRatio { get; set; } = new(0.6F, 0.1F, 0F);

        protected virtual void Update()
        {
            transform.position = Anchor.position;
            transform.LookAt(Face);
        }

        public void SetColor(Color color)
        {
            TooltipLine.SetColor(color);
            Text.color = color;
        }
    }
}