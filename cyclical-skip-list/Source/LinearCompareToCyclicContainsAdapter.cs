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
            if (_linearCompare(a, b) == 0 || _linearCompare(a, c) == 0)
            {
                return true;
            }
            else
            {
                return L(a, b) && L(b, c) ||
                       L(b, c) && L(c, a) ||
                       L(c, a) && L(a, b);        
            }
        }

        private bool L(T a, T b)
        {
            return _linearCompare(a, b) < 0;
        }
    }
}
