using System;
using Naukri.InspectorMaid;
using Naukri.Moltk.Utility;
using Naukri.Physarum;
using UnityEngine;
using UnityEngine.UIElements;

namespace Naukri.Moltk.Outline.Tooltip
{
    public class TooltipTrigger : Consumer.Behaviour
    {
        [Flags]
        public enum Trigger
        {
            Highlight = Flag._00,

            Hover = Flag._01,

            Select = Flag._02
        }

        [SerializeField]
        [Target]
        [RowScope]
        [ShowIf(nameof(tooltip), null)]
        [HelpBox("Tooltip not assigned. Please assign a tooltip or pressing the button below to create a new tooltip.",
            HelpBoxMessageType.Error)]
        [Button("Create Tooltip", nameof(CreateTooltip))]
        [EndScope]
        private Tooltip tooltip;

        [SerializeField]
        private Trigger trigger = Trigger.Highlight | Trigger.Hover | Trigger.Select;

        [Target]
        [RowScope]
        [HideIf(nameof(IsTooltipTextMatched))]
        [HelpBox("Tooltip text miss matched.", HelpBoxMessageType.Warning)]
        [Style(flexGrow: "1")]
        [Button("Fix", nameof(MatchTooltipText))]
        [EndScope]
        [TextArea(3, 5)]
        public string tooltipText;

        protected override void Build()
        {
            var outlineService = ctx.Watch<OutlineService>();

            if (outlineService.IsSelected(gameObject))
            {
                if (trigger.HasFlag(Trigger.Select))
                {
                    ShowTooltip();
                    tooltip.SetColor(outlineService.SelectColor);
                }
                else
                {
                    HideTooltip();
                }
            }
            else if (outlineService.IsHovered(gameObject))
            {
                if (trigger.HasFlag(Trigger.Hover))
                {
                    ShowTooltip();
                    tooltip.SetColor(outlineService.HoverColor);
                }
                else
                {
                    HideTooltip();
                }
            }
            else if (outlineService.IsHighlighted(gameObject))
            {
                if (trigger.HasFlag(Trigger.Highlight))
                {
                    ShowTooltip();
                    tooltip.SetColor(outlineService.HighlightColor);
                }
                else
                {
                    HideTooltip();
                }
            }
            else
            {
                HideTooltip();
            }
        }

        protected override void HandleEvent(object sender, IElementEvent evt)
        {
            base.HandleEvent(sender, evt);

            if (evt is OutlineCollectionChangedEvent outlineCollectionChangedEvent)
            {
                if (outlineCollectionChangedEvent.target == gameObject)
                {
                    ctx.Dispatch(ElementEvents.Refresh.Default);
                }
            }
        }

        [Template]
        private void CreateTooltip()
        {
            var tooltipService = FindAnyObjectByType<TooltipService>()
                                 ?? new GameObject("[Tooltip Service]").AddComponent<TooltipService>();

            var tooltipAnchor = transform.Find("[Tooltip Anchor]");

            if (tooltipAnchor == null)
            {
                tooltipAnchor = new GameObject("[Tooltip Anchor]").transform;
                tooltipAnchor.SetParent(transform);
                tooltipAnchor.transform.localPosition = Vector3.zero;
                tooltipAnchor.transform.localRotation = Quaternion.identity;
                tooltipAnchor.transform.localScale = Vector3.one;
            }

            tooltip = tooltipService.CreateTooltip(tooltipAnchor, Camera.main.transform, tooltipText, Color.white);
            tooltip.gameObject.name = $"{gameObject.name}'s Tooltip";
        }

        private bool IsTooltipTextMatched()
        {
            if (tooltip == null)
            {
                return false;
            }

            return tooltip.Text.text == tooltipText;
        }

        private void MatchTooltipText()
        {
            tooltip.Text.text = tooltipText;
        }

        private void ShowTooltip()
        {
            if (tooltip != null)
            {
                tooltip.gameObject.SetActive(true);
            }
        }

        private void HideTooltip()
        {
            if (tooltip == null)
            {
                return;
            }

            tooltip.gameObject.SetActive(false);
        }
    }
}