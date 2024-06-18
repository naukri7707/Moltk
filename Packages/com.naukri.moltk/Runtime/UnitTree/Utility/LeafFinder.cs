using System;
using System.Collections.Generic;
using UnityEngine;

namespace Naukri.Moltk.UnitTree.Utility
{
    internal static class LeafFinder
    {
        public static Transform FindNextLeaf(Transform target)
        {
            // If the node is a leaf node,
            // go back to the parent node of the subtree and find the leaf node of the next subtree
            if (target.childCount == 0)
            {
                return FindNextLeafInSiblingOrParent(target);
            }
            else
            {
                throw new Exception("target must be leaf node");
            }
        }

        public static Transform FindPreviousLeaf(Transform currentLeaf)
        {
            // If the node is a leaf node,
            // go back to the parent node of the subtree and find the leaf node of the previous subtree
            if (currentLeaf.childCount == 0)
            {
                return FindPreviousLeafInSiblingOrParent(currentLeaf);
            }
            else
            {
                throw new Exception("target must be leaf node");
            }
        }

        public static Transform FindLowestCommonAncestor(Transform left, Transform right)
        {
            // Create a list of ancestors of the left node
            var leftAncestors = new HashSet<Transform>();
            while (left != null)
            {
                leftAncestors.Add(left);
                left = left.parent;
            }

            // Find the first ancestor of the right node that appears in the ancestor list of the left node
            while (right != null)
            {
                if (leftAncestors.Contains(right))
                {
                    return right;
                }

                right = right.parent;
            }

            return null;
        }

        public static Transform FindFirstLeaf(Transform node)
        {
            Transform current = node;

            while (current.childCount > 0)
            {
                current = current.GetChild(0);
            }

            return current;
        }

        public static Transform FindLastLeaf(Transform node)
        {
            Transform current = node;

            while (current.childCount > 0)
            {
                int count = current.childCount;
                current = current.GetChild(count - 1);
            }

            return current;
        }

        public static IEnumerable<Transform> FindAllLeaf(Transform node)
        {
            if (node.childCount == 0)
            {
                yield return node;
            }
            else
            {
                for (int i = 0; i < node.childCount; i++)
                {
                    var child = node.GetChild(i);

                    foreach (var leaf in FindAllLeaf(child))
                    {
                        yield return leaf;
                    }
                }
            }
        }

        private static Transform FindNextLeafInSiblingOrParent(Transform currentLeaf)
        {
            Transform parent = currentLeaf.parent;

            // Searching for the leaf node of the next subtree from the parent node
            while (parent != null)
            {
                int index = currentLeaf.GetSiblingIndex();

                if (index < parent.childCount - 1)
                {
                    var nextChild = parent.GetChild(index + 1);
                    return FindFirstLeaf(nextChild);
                }

                currentLeaf = parent;
                parent = currentLeaf.parent;
            }

            return null;
        }

        private static Transform FindPreviousLeafInSiblingOrParent(Transform currentLeaf)
        {
            Transform parent = currentLeaf.parent;

            // Searching for the leaf node of the previous subtree from the parent node
            while (parent != null)
            {
                int index = currentLeaf.GetSiblingIndex();

                if (index > 0)
                {
                    var previousChild = parent.GetChild(index - 1);
                    return FindLastLeaf(previousChild);
                }

                currentLeaf = parent;
                parent = currentLeaf.parent;
            }

            return null;
        }
    }
}
