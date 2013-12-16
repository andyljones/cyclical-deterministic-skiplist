namespace CyclicalSkipList
{
    public class Node<T> : INode<T>
    {
        public T Key { get; set; }

        public INode<T> Right { get; set; }
        public INode<T> Left { get; set; }
        public INode<T> Up { get; set; }
        public INode<T> Down { get; set; }

        public Node(T key)
        {
            Key = key;
        }
    }
}
