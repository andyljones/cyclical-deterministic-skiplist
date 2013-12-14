namespace CyclicalSkipList
{
    public class Node<T> : INode<T>
    {
        public T Key { get; set; }
        public INode<T> Right { get; set; }
        public INode<T> Down { get; set; }

        public Node(T key)
        {
            Key = key;
        }
    }
}
