namespace CyclicalSkipList
{
    public static class SkiplistUtilities
    {
        public static INode<T> CreateNode<T>(T key)
        {
            return new Node<T> { Key = key };
        }
    }
}
