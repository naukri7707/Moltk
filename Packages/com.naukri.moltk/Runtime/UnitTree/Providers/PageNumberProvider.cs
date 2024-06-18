using Naukri.InspectorMaid;
using Naukri.Moltk.MVU;
using Naukri.Moltk.UnitTree.Events;
using Naukri.Moltk.UnitTree.Utility;
using System;
using System.Linq;
using UnityEngine;

namespace Naukri.Moltk.UnitTree.Providers
{
    public record PageNumber(int Current, int Total) : State
    {
        public PageNumber() : this(
            Current: 0,
            Total: 0
            )
        {
        }

        public string PageNumberText => $"{(Current + 1).ToString().PadLeft(Total.ToString().Length, '0')} / {Total}";
    }

    public class PageNumberProvider : ProviderBehaviour<PageNumber>
    {
        [
           Slot(nameof(PageNumberText)),
           Target, ReadOnly
        ]
        public Transform[] pages;

        [Template]
        public string PageNumberText => State.PageNumberText;

        protected override void Build(IProvider provider)
        {
            // Do nothing
        }

        protected override void Start()
        {
            base.Start();
            pages = GetAllPage();
            State = State with
            {
                Total = pages.Length,
            };
        }

        protected override void OnEnable()
        {
            base.Awake();
            var controller = UnitTreeController.Of(this);
            controller.EventHandler.AddListener(HandleEvent);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            var controller = UnitTreeController.Of(this);
            controller.EventHandler.RemoveListener(HandleEvent);
        }

        protected virtual Transform[] GetAllPage()
        {
            return LeafFinder.FindAllLeaf(transform).ToArray();
        }

        protected virtual int GetNodeIndex(Transform node)
        {
            return Array.IndexOf(pages, node);
        }

        private void HandleEvent(UnitTreeEvent unitTreeEvent)
        {
            if (unitTreeEvent is NodeChangedEvent nodeChangedEvent)
            {
                var idx = GetNodeIndex(nodeChangedEvent.to.transform);
                State = State with
                {
                    Current = idx,
                };
            }
        }
    }
}
