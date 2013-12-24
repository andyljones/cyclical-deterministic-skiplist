namespace CyclicalSkipList
{
    public interface ICyclicContains<T>
    {
        bool CyclicContains(T a, T b, T c);
    }
}
