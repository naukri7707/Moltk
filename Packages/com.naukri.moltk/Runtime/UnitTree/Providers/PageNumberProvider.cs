using System;
using System.Linq;
using Naukri.Moltk.UnitTree.Events;
using Naukri.Moltk.UnitTree.Utility;
using Naukri.Physarum;
using UnityEngine;

namespace Naukri.Moltk.UnitTree.Providers
{
    public record PageNumberState(int CurrentPage = 0, int TotalPage = 0)
    {
        public string PageNumberText => $"{CurrentPage + 1}/{TotalPage}";
    }

    public class PageNumberProvider : StateProvider<PageNumberState>.Behaviour
    {
        private Transform[] pages;

        [SerializeField]
        private UnitTreeController unitTreeController;

        protected override void Awake()
        {
            base.Awake();
            pages = GetAllPage();
        }

        protected override void Start()
        {
            base.Start();
            SetState(s => s with { TotalPage = pages.Length });
        }

        protected virtual Transform[] GetAllPage()
        {
            return LeafFinder.FindAllLeaf(transform).ToArray();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            unitTreeController.EventHandler.AddListener(HandleTreeEvent);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            unitTreeController.EventHandler.RemoveListener(HandleTreeEvent);
        }

        protected virtual int GetNodeIndex(Transform node)
        {
            return Array.IndexOf(pages, node);
        }

        private void HandleTreeEvent(UnitTreeEvent evt)
        {
            if (evt is NodeChangedEvent nodeChangedEvent)
            {
                var idx = GetNodeIndex(nodeChangedEvent.to.transform);
                SetState(s => s with { CurrentPage = idx, });
            }
        }

        protected override PageNumberState Build()
        {
            var state = State ?? new();
            return state;
        }
    }
}
