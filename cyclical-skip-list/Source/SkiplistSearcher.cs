using System;

namespace CyclicalSkipList
{
    public static class SkiplistSearcher
    {
        public static bool Find<T>(
            this Skiplist<T> skiplist, 
            T key, 
            INode<T> origin = null, 
            Action<INode<T>> pathAction = null)
        {
            var gapContainingNode = skiplist.FetchGap(key, origin, pathAction);

            return Equals(gapContainingNode.Key, key);
        }

        public static INode<T> FetchGap<T>(
            this Skiplist<T> skiplist, 
            T key, 
            INode<T> origin = null,
            Action<INode<T>> pathAction = null)
        {
            if (skiplist.Head == null)
            {
                return null;
            }

            var node = origin ?? skiplist.Head;

            var atCorrectNode = false;
            while (!atCorrectNode)
            {
                node = FindCorrectGapInLevel(key, node, skiplist.InOrder);

                if (pathAction != null)
                {
                    pathAction(node);
                }

                if (node.Down != null)
                {
                    node = node.Down;
                }
                else
                {
                    atCorrectNode = true;
                }
            }

            return node;
        }

        private static INode<T> FindCorrectGapInLevel<T>(T key, INode<T> start, Func<T, T, T, bool> compare)
        {
            var node = start;

            while (!compare(node.Key, key, node.Right.Key))
            {
                node = node.Right;
            }

            return node;
        }
    }
}
