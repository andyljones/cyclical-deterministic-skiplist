namespace CyclicalSkipList
{
    public interface INode<T>
    {
        T Key { get; set; }
        INode<T> Right { get; set; }
        INode<T> Left { get; set; } 
        INode<T> Down { get; set; }
        INode<T> Up { get; set; } 
    }
}
