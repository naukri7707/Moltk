using System;
using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using Naukri.Moltk.UnitTree.Events;
using Naukri.Moltk.UnitTree.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Naukri.Moltk.UnitTree
{
    /// <summary>
    /// Control the unit tree behavior and navigation.
    /// </summary>
    public partial class UnitTreeController : UnitTreeBehaviour
    {
        [SerializeField]
        private bool startImmediately = true;

        [
            SerializeField,
            ShowIf(nameof(startImmediately)),
            Target,
            Slot(nameof(startNode))
        ]
        private bool customStartNode;

        [
            Template,
            ShowIf(nameof(customStartNode)),
            Target,
            HelpBox("startNode can not be null.", HelpBoxMessageType.Error), ShowIf(nameof(startNode), null),
        ]
        [SerializeField]
        private Transform startNode;

        private Transform currentNode;

        [SerializeField]
        private UnityEvent<UnitTreeEvent> eventHandler;

        [Template]
        public Transform CurrentNode => currentNode;

        public UnityEvent<UnitTreeEvent> EventHandler => eventHandler;

        /// <summary>
        /// Rolls back to the previous leaf node.
        /// </summary>
        /// <returns><c>true</c> if the rollback was successful; otherwise, <c>false</c>.</returns>
        public bool RollBack()
        {
            var previous = LeafFinder.FindPreviousLeaf(currentNode);

            if (previous == null)
            {
                return false;
            }

            MoveToImpl(previous);

            return true;
        }

        /// <summary>
        /// Moves to the next leaf node.
        /// </summary>
        /// <returns><c>true</c> if the move was successful; otherwise, <c>false</c>.</returns>
        public bool MoveNext()
        {
            var next = LeafFinder.FindNextLeaf(currentNode);

            if (next == null)
            {
                return false;
            }

            MoveToImpl(next);

            return true;
        }

        [Template]
        /// <summary>
        /// Moves to the specified target node.
        /// </summary>
        /// <param name="target">The target node to move to.</param>
        public void MoveTo(Transform target)
        {
            MoveToImpl(target);
        }

        protected override void HandleTreeEvent(UnitTreeEvent evt)
        {
            base.HandleTreeEvent(evt);
            eventHandler.Invoke(evt);
        }

        protected virtual void Start()
        {
            if (startImmediately)
            {
                var node = customStartNode
                ? startNode
                : LeafFinder.FindFirstLeaf(transform);

                MoveToImpl(node);
            }
        }

        private void MoveToImpl(Transform target)
        {
            if (target != null && target.childCount > 0)
            {
                throw new Exception("target must be leaf node");
            }
            var lca = LeafFinder.FindLowestCommonAncestor(currentNode, target);

            // Send OnNodeChanging
            var nodeChangingEvent = new NodeChangingEvent(
                currentNode != null ? currentNode.gameObject : null,
                target != null ? target.gameObject : null
                );
            BroadcastMessage(nameof(HandleTreeEvent), nodeChangingEvent, SendMessageOptions.DontRequireReceiver);

            // Deactivate nodes from current to LCA and activate nodes from LCA to target
            ExitNodesToLCA(currentNode, lca);
            EnterNodesFromLCA(target, lca);

            // Update currentNode
            currentNode = target;

            // Send OnNodeChanged
            var nodeChangedEvent = new NodeChangedEvent(
                currentNode != null ? currentNode.gameObject : null,
                target != null ? target.gameObject : null
                );
            BroadcastMessage(nameof(HandleTreeEvent), nodeChangedEvent, SendMessageOptions.DontRequireReceiver);
        }

        private void ExitNodesToLCA(Transform node, Transform lca)
        {
            while (node != lca)
            {
                node.SendMessage(nameof(InvokeExit), SendMessageOptions.DontRequireReceiver);
                node = node.parent;
            }
        }

        private void EnterNodesFromLCA(Transform node, Transform lca)
        {
            if (node == lca)
            {
                return;
            }
            EnterNodesFromLCA(node.parent, lca);
            node.SendMessage(nameof(InvokeEnter), SendMessageOptions.DontRequireReceiver);
        }
    }

    [
        HelpBox(
            "UnitTreeController is considered as the root node and is used to control the behavior and navigation of the UnitTree.",
            HelpBoxMessageType.Info
            ), Style(marginBottom: "4"),
        ScriptField,
        Base,
        Members,
    ]
    partial class UnitTreeController
    {
        /// <summary>
        /// Gets the UnitTreeManager of target component.
        /// </summary>
        /// <param name="component">The component to get the UnitTreeManager from.</param>
        /// <returns>The UnitTreeManager.</returns>
        public static UnitTreeController Of(Component component)
        {
            return component.GetComponentInParent<UnitTreeController>(true);
        }
    }
}
