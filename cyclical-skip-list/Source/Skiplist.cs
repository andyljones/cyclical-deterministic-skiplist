namespace CyclicalSkipList
{
    public class Skiplist<T>
    {
        public INode<T> Head;

        public readonly int MinimumGapSize = 2;

        public override string ToString()
        {
            return SkiplistStringFormatter.StringOf(this);
        }
    }
}
