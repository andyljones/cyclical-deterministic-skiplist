namespace CyclicalSkipList
{
    public class Skiplist<T>
    {
        public INode<T> Head;

        public override string ToString()
        {
            return SkiplistStringFormatter.StringOf(this);
        }
    }
}
