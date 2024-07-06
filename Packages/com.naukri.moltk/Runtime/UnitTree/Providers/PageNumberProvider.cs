using Naukri.InspectorMaid;
using Naukri.Moltk.Fusion;
using Naukri.Moltk.UnitTree.Events;
using Naukri.Moltk.UnitTree.Utility;
using System;
using System.Linq;
using UnityEngine;

namespace Naukri.Moltk.UnitTree.Providers
{
    public record PageNumber(int Current = 0, int Total = 0)
    {
        public string PageNumberText => $"{(Current + 1).ToString().PadLeft(Total.ToString().Length, '0')} / {Total}";
    }

    public class PageNumberProvider : Provider<PageNumber>
    {
        [ReadOnly]
        public Transform[] pages;

        public string PageNumberText => State.PageNumberText;

        protected override PageNumber Build(PageNumber state)
        {
            return state ?? new PageNumber();
        }

        protected override void Start()
        {
            base.Start();
            pages = GetAllPage();
            SetState(s => s with
            {
                Total = pages.Length,
            });
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
                SetState(s => s with
                {
                    Current = idx,
                });
            }
        }
    }
}
