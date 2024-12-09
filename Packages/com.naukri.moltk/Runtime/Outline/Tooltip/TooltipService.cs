using Naukri.InspectorMaid;
using Naukri.Physarum;
using UnityEngine;

namespace Naukri.Moltk.Outline.Tooltip
{
    public class TooltipService : Provider.Behaviour
    {
        public Tooltip prefab;

        [Target]
        public Tooltip CreateTooltip(Transform anchor, Transform face, string text, Color color)
        {
            var tooltip = Instantiate(prefab);
            tooltip.Anchor = anchor;
            tooltip.Face = face;
            // line
            tooltip.TooltipLine.SetColor(color);
            // text
            tooltip.Text.text = text;
            tooltip.Text.color = color;
            return tooltip;
        }
    }
}
