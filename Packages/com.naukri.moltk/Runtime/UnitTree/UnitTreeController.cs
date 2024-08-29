using System;
using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using Naukri.Moltk.UnitTree.Events;
using Naukri.Moltk.UnitTree.Utility;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Naukri.Moltk.UnitTree
{
    /// <summary>
    /// Control the unit tree behavior and navigation.
    /// </summary>
    public partial class UnitTreeController : UnitTreeBehaviour
    {
        #region fields

        [SerializeField]
        private bool startImmediately = true;

        [SerializeField, ShowIf(nameof(startImmediately)), Target, Slot(nameof(startNode))]
        private bool customStartNode;

        [
            Template,
            ShowIf(nameof(customStartNode)),
            Target,
            HelpBox("startNode can not be null.", HelpBoxMessageType.Error),
            ShowIf(nameof(startNode), null),
        ]
        [SerializeField]
        private Transform startNode;

        private Transform currentNode;

        [SerializeField]
        private UnityEvent<UnitTreeEvent> eventHandler;

        #endregion

        [Template]
        public Transform CurrentNode => currentNode;

        public UnityEvent<UnitTreeEvent> EventHandler => eventHandler;

        #region methods

        public Transform GetNext()
        {
            if (currentNode == null)
            {
                return null;
            }

            var target = LeafFinder.FindNextLeaf(currentNode);

            if (!IsChild(target))
            {
                return null;
            }

            return target;
        }

        public Transform GetPrevious()
        {
            if (currentNode == null)
            {
                return null;
            }

            var target = LeafFinder.FindPreviousLeaf(currentNode);

            if (!IsChild(target))
            {
                return null;
            }

            return target;
        }

        /// <summary>
        /// Rolls back to the previous leaf node.
        /// </summary>
        /// <returns><c>true</c> if the rollback was successful; otherwise, <c>false</c>.</returns>
        public bool RollBack()
        {
            var target = GetPrevious();

            if (target == null)
            {
                return false;
            }

            MoveToImpl(target);
            return true;
        }

        /// <summary>
        /// Moves to the next leaf node.
        /// </summary>
        /// <returns><c>true</c> if the move was successful; otherwise, <c>false</c>.</returns>
        public bool MoveNext()
        {
            var target = GetNext();

            if (target == null)
            {
                return false;
            }

            MoveToImpl(target);
            return true;
        }

        /// <summary>
        /// Gets the UnitTreeManager of target component.
        /// </summary>
        /// <param name="component">The component to get the UnitTreeManager from.</param>
        /// <returns>The UnitTreeManager.</returns>
        public static UnitTreeController Of(Component component)
        {
            Assert.IsNotNull(component);
            return component.GetComponentInParent<UnitTreeController>(true);
        }

        protected override void HandleTreeEvent(UnitTreeEvent evt)
        {
            base.HandleTreeEvent(evt);
            eventHandler.Invoke(evt);
        }

        protected override void Start()
        {
            base.Start();
            if (startImmediately)
            {
                var node = customStartNode ? startNode : LeafFinder.FindFirstLeaf(transform);

                MoveToImpl(node);
            }
        }

        [Template]
        /// <summary>
        /// Moves to the specified target node.
        /// </summary>
        /// <param name="target">The target node to move to.</param>
        public void MoveTo(Transform target)
        {
            Assert.IsNotNull(target);

            if (!IsChild(target))
            {
                throw new OperationCanceledException(
                    $"'{target.name}' must be a child of controller '{name}'"
                );
            }
            MoveToImpl(target);
        }

        private bool IsChild(Transform target)
        {
            var current = target;
            while (current != null)
            {
                if (current == transform)
                {
                    return true;
                }
                current = current.parent;
            }
            return false;
        }

        private void MoveToImpl(Transform target)
        {
            Assert.IsNotNull(target);

            if (target != null && target.childCount > 0)
            {
                throw new OperationCanceledException($"{target.name} must be leaf node");
            }
            var lca = LeafFinder.FindLowestCommonAncestor(currentNode, target);

            // Send OnNodeChanging
            var nodeChangingEvent = new NodeChangingEvent(
                currentNode != null ? currentNode.gameObject : null,
                target != null ? target.gameObject : null
            );
            BroadcastMessage(
                nameof(HandleTreeEvent),
                nodeChangingEvent,
                SendMessageOptions.DontRequireReceiver
            );

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
            BroadcastMessage(
                nameof(HandleTreeEvent),
                nodeChangedEvent,
                SendMessageOptions.DontRequireReceiver
            );
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

        #endregion
    }
    /// <summary>
    /// Gets the UnitTreeManager of target component.
    /// </summary>
    /// <param name="component">The component to get the UnitTreeManager from.</param>
    /// <returns>The UnitTreeManager.</returns>
}
