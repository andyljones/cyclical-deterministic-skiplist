namespace CyclicalSkipList
{
    public interface ICyclicComparer<T>
    {
        bool CyclicCompare(T a, T b, T c);
    }
}
