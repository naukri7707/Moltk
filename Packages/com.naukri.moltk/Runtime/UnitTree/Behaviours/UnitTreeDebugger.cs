using System.Linq;
using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using Naukri.Moltk.UnitTree.Events;
using Naukri.Moltk.UnitTree.Utility;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Naukri.Moltk.UnitTree.Behaviours
{
    [RequireComponent(typeof(UnitTreeController))]
    public partial class UnitTreeDebugger : UnitTreeBehaviour
    {
        public bool pingCurrentNode = true;

        public string[] tierNames;

        [Rename("from", useNicifyName: true)]
        public Transform selectRoot;

        private UnitTreeController unitTreeController;

        public enum HelperMethod
        {
            LeafSelector,

            SetTierName,
        }

        [Target]
        public HelperMethod TargetHelper { get; set; }

        [Template]
        protected Transform CurrentNode => unitTreeController != null ? unitTreeController.CurrentNode : null;

        protected bool HasNode => CurrentNode != null;

        public void SetTierName()
        {
            void DFS(Transform transform, int depth)
            {
                foreach (Transform child in transform)
                {
                    DFS(child, depth + 1);
                    var index = child.GetSiblingIndex();
                    child.name = $"{tierNames[depth]} {index + 1}";
                }
            }
            DFS(transform, 0);
        }

        [Template]
        public void SelectAllLeaf()
        {
#if UNITY_EDITOR
            var leaves = LeafFinder.FindAllLeaf(selectRoot).Select(it => it.gameObject).ToArray();
            UnityEditor.Selection.objects = leaves;
#endif
        }

        [Template]
        public void SelectFirstLeaf()
        {
#if UNITY_EDITOR
            var leaf = LeafFinder.FindFirstLeaf(selectRoot).gameObject;
            UnityEditor.Selection.activeObject = leaf;
#endif
        }

        [Template]
        public void SelectLastLeaf()
        {
#if UNITY_EDITOR
            var leaf = LeafFinder.FindLastLeaf(selectRoot).gameObject;
            UnityEditor.Selection.activeObject = leaf;
#endif
        }

        [Template]
        protected void MoveNextNode()
        {
            unitTreeController.MoveNext();
        }

        [Template]
        protected void RollBackNode()
        {
            unitTreeController.RollBack();
        }

        [Template]
        protected void MoveToNode(Transform target)
        {
            unitTreeController.MoveTo(target);
        }

        protected override void HandleTreeEvent(UnitTreeEvent evt)
        {
            if (pingCurrentNode)
            {
                if (evt is NodeChangedEvent)
                {
                    PingCurrentNode();
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            unitTreeController = GetComponent<UnitTreeController>();
        }

        private void PingCurrentNode()
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUIUtility.PingObject(CurrentNode.gameObject);
#endif
        }
    }

    [
        ScriptField,
        Base,
        GroupScope("Node Control"),
            ColumnScope, EnableIf(nameof(HasNode)),
                RowScope,
                    Slot(nameof(CurrentNode)), Style(flexGrow: "1"),
                    Button("Ping", nameof(PingCurrentNode)),
                EndScope,
                RowScope, Style(height: "25"),
                    Button("RollBack", nameof(RollBackNode)), Style(flexGrow: "1"),
                    Button("MoveNext", nameof(MoveNextNode)), Style(flexGrow: "1"),
                EndScope,
                Slot(nameof(MoveToNode)),
            EndScope,
        EndScope,
        GroupScope("Debugger Settings"),
            Slot(nameof(pingCurrentNode)),
            ColumnScope, ShowIf(nameof(pingCurrentNode)),
            EndScope,
        EndScope,
        GroupScope("Helpers"),
            Slot(nameof(TargetHelper)),
            ColumnScope, ShowIf(nameof(TargetHelper), HelperMethod.LeafSelector),
                Slot(nameof(selectRoot)),
                RowScope,
                    Button("First", nameof(SelectFirstLeaf)), Style(flexGrow: "1"),
                    Button("Last", nameof(SelectLastLeaf)), Style(flexGrow: "1"),
                    Button("All", nameof(SelectAllLeaf)), Style(flexGrow: "1"),
                EndScope,
            EndScope,
            ColumnScope, ShowIf(nameof(TargetHelper), HelperMethod.SetTierName),
                Slot(nameof(tierNames)),
                Button("Set Tier Name", nameof(SetTierName)),
            EndScope,
        EndScope,
    ]
    partial class UnitTreeDebugger
    { }
}
