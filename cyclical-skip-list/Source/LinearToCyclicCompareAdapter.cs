using System;
using System.Collections;
using System.Collections.Generic;

namespace CyclicalSkipList
{
    public class LinearToCyclicCompareAdapter<T> : ICyclicComparer<T>
    {
        private readonly Func<T, T, int> _linearCompare; 

        public LinearToCyclicCompareAdapter(Func<T, T, int> linearCompare)
        {
            _linearCompare = linearCompare;
        }

        public bool CyclicCompare(T a, T b, T c)
        {
            return Leq(a, b) && Leq(b, c) || 
                   Leq(b, c) && Leq(c, a) || 
                   Leq(c, a) && Leq(a, b);
        }

        private bool Leq(T a, T b)
        {
            return _linearCompare(a, b) <= 0;
        }
    }
}
