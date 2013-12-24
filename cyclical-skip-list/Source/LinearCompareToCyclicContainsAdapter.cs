using System;
using System.Collections;
using System.Collections.Generic;

namespace CyclicalSkipList
{
    public class LinearCompareToCyclicContainsAdapter<T> : ICyclicContains<T>
    {
        private readonly Func<T, T, int> _linearCompare; 

        public LinearCompareToCyclicContainsAdapter(Func<T, T, int> linearCompare)
        {
            _linearCompare = linearCompare;
        }

        public bool CyclicContains(T a, T b, T c)
        {
            if (_linearCompare(a, c) == 0)
            {
                return true;
            }
            else
            {
                return LEq(a, b) && L(b, c) ||
                   LEq(b, c) && L(c, a) ||
                   LEq(c, a) && L(a, b);        
            }
        }

        private bool LEq(T a, T b)
        {
            return _linearCompare(a, b) <= 0;
        }

        private bool L(T a, T b)
        {
            return _linearCompare(a, b) < 0;
        }
    }
}
