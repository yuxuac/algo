using System;
using System.Collections.Generic;

namespace Algo
{
    public class TreeNode
    {
        public TreeNode(int input)
        {
            this.Value = input;
        }

        public int? Value { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
    }

    public class MyBinarySearchTree
    {
        private TreeNode root;
        public MyBinarySearchTree()
        {
            this.root = null;
        }

        public TreeNode GetRoot()
        {
            return this.root;
        }

        public TreeNode Insert(int val)
        {
            var currentNode = this.root;

            if (this.root == null)
            {
                this.root = new TreeNode(val);
            }
            else
            {
                while (true)
                {
                    if (currentNode.Value > val)
                    {
                        if (currentNode.Left == null)
                        {
                            currentNode.Left = new TreeNode(val);
                            break;
                        }
                        else
                            currentNode = currentNode.Left;
                    }
                    else if (currentNode.Value < val)
                    {
                        if (currentNode.Right == null)
                        {
                            currentNode.Right = new TreeNode(val);
                            break;
                        }
                        else
                            currentNode = currentNode.Right;
                    }
                    else
                    {
                        currentNode = null;
                        break;
                    }
                }
            }

            return currentNode;
        }

        public Tuple<TreeNode, TreeNode> Lookup(int val)
        {
            if (this.root == null)
            {
                return null;
            }
            else
            {
                var currentNode = this.root;
                TreeNode currentParentNode = null;

                while (true)
                {
                    if (currentNode.Value > val)
                    {
                        currentParentNode = currentNode;
                        currentNode = currentNode.Left;
                    }
                    else
                    {
                        currentParentNode = currentNode;
                        currentNode = currentNode.Right;
                    }

                    // Can't find any.
                    if (currentNode == null)
                        break;

                    // Find one.
                    if (currentNode.Value == val)
                        return Tuple.Create<TreeNode, TreeNode>(currentNode, currentParentNode);
                }
            }
            return null;
        }

        public bool Remove(int val)
        {
            // 1. No children, delete it directly
            // 2. Has one child, delete it and link parent to its child
            // 3. the node Has 2 children, right child's left child (keep looping until it has no left child)
            //    the node's parent's right child -> the target node (delink the parent)
            //    the target node's parent's left child = null (delete the target node in original position)
            //    the target node's children = the node's children (left and right, delink the children)
            var targetNode = Lookup(val);
            var currentNode = targetNode.Item1;
            var currentParentNode = targetNode.Item2;

            if (targetNode != null)
            {
                // 1. No children, delete it directly
                if (currentNode.Left == null && currentNode.Right == null)
                {
                    if (currentParentNode.Left.Value == currentNode.Value)
                        currentParentNode.Left = null;
                    else if (currentParentNode.Right.Value == currentNode.Value)
                        currentParentNode.Right = null;
                }
                // 3. the node Has 2 children
                else if (currentNode.Left != null && currentNode.Right != null)
                {
                    var firstRight = currentNode.Right;
                    var currentTarget = firstRight;
                    TreeNode currentTargetParent = currentNode;
                    while (true)
                    {
                        if (currentTarget.Left == null)
                            break;
                        currentTargetParent = currentTarget;
                        currentTarget = currentTarget.Left;
                    }

                    // the node's parent's right child -> the target node (delink the parent)
                    currentParentNode.Right = currentTarget;
                    currentTargetParent.Left = currentTarget.Right;
                    currentTarget.Right = firstRight;
                    currentTarget.Left = currentNode.Left;
                }
                // 2. Has one child, delete it and link parent to its child
                else if (currentNode.Left != null)
                {
                    if (currentParentNode.Left.Value == currentNode.Value)
                        currentParentNode.Left = currentNode.Left;
                    else if (currentParentNode.Right.Value == currentNode.Value)
                        currentParentNode.Right = currentNode.Left;
                }
                else if (currentNode.Right != null)
                {
                    if (currentParentNode.Left.Value == currentNode.Value)
                        currentParentNode.Left = currentNode.Right;
                    else if (currentParentNode.Right.Value == currentNode.Value)
                        currentParentNode.Right = currentNode.Right;
                }
                return true;
            }
            return false;
        }

        public void BFS()
        {
            var currentNode = this.root;
            var queue = new Queue<TreeNode>();
            List<TreeNode> result = new List<TreeNode>();
            queue.Enqueue(currentNode);

            while (queue.Count > 0)
            {
                currentNode = queue.Dequeue();
                Console.Write(currentNode.Value + "->");
                result.Add(currentNode);
                if (currentNode.Left != null)
                    queue.Enqueue(currentNode.Left);
                if (currentNode.Right != null)
                    queue.Enqueue(currentNode.Right);
            }
        }

        public List<TreeNode> BFSResursive(Queue<TreeNode> queue, List<TreeNode> result)
        {
            if (queue.Count <= 0)
                return result;

            var currentNode = queue.Dequeue();
            result.Add(currentNode);
            Console.Write(currentNode.Value + "->");
            if (currentNode.Left != null)
                queue.Enqueue(currentNode.Left);
            if (currentNode.Right != null)
                queue.Enqueue(currentNode.Right);

            return BFSResursive(queue, result);
        }

        /*
             9
         4      20
       1   6  15  170

        InOrder (left-mid-right) [1, 4, 6, 9, 15, 20, 170]
        PreOrder (mid-left-right) [9, 4, 1, 6, 20, 15, 170]
        PostOrder (left-right-mid) [1, 6, 4, 15, 170, 20, 9]
        */

        // Space complexity is the height of the tree which is also the height of the call stack.
        public void DFS_InOrder(TreeNode node)
        {
            if (node == null) return;

            if (node.Left != null)
                DFS_InOrder(node.Left);
            Console.WriteLine(node.Value + "->");
            if (node.Right != null) 
                DFS_InOrder(node.Right);
        }

        public void DFS_PreOrder(TreeNode node)
        {
            if (node == null) return;

            Console.WriteLine(node.Value + "->");
            if (node.Left != null)
                DFS_PreOrder(node.Left);
            if (node.Right != null)
                DFS_PreOrder(node.Right);
        }

        public void DFS_PostOrder(TreeNode node)
        {
            if (node == null) return;
            
            if (node.Left != null)
                DFS_PostOrder(node.Left);
            if (node.Right != null)
                DFS_PostOrder(node.Right);
            Console.WriteLine(node.Value + "->");
        }
    }
}