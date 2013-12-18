using System.Linq;

namespace CyclicalSkipList
{
    public static class SkiplistSearcher
    {
        public static bool Contains<T>(this Skiplist<T> skiplist, T key)
        {
            var currentNode = skiplist.Head;
            bool atCorrectNode = false;
            while (!atCorrectNode)
            {
                currentNode = skiplist.FindGapContainingKey(key, currentNode);

                if (currentNode.Down != null)
                {
                    currentNode = currentNode.Down;
                }
                else
                {
                    atCorrectNode = true;
                }
            }

            return (skiplist.KeyEquals(key, currentNode.Key));
        }

        public static INode<T> FindGapContainingKey<T>(this Skiplist<T> skiplist, T key, INode<T> nodeInLevel)
        {
            var ends = SkiplistUtilities.EnumerateLevels(skiplist.Head).ToList();

            var currentNode = nodeInLevel;
            while (skiplist.KeyCompare(key, currentNode.Key) > 0 && !ends.Contains(currentNode.Right))
            {
                currentNode = currentNode.Right;
            }

            return currentNode;
        }
    }
}
