using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;
using NSubstitute;
using Ploeh.AutoFixture;

namespace CyclicalSkipListTests
{
    public class RandomRepeatCountCustomization : ICustomization
    {
        private readonly int _upperBound;
        private readonly int _lowerBound;

        public RandomRepeatCountCustomization(int lowerBound, int upperBound)
        {
            _lowerBound = lowerBound;
            _upperBound = upperBound;
        }

        public void Customize(IFixture fixture)
        {
            fixture.RepeatCount = new Random().Next(_lowerBound, _upperBound);
        }
    }

    public class IsolatedNodeCustomization<T> : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var node = Substitute.For<INode<T>>();
                node.Key = fixture.Create<T>();
                node.Right = null;
                node.Down = null;
                return node;
            });
        }
    }

    public class DistinctIntCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Freeze<List<int>>();
            
            var ints = fixture.Create<List<int>>();
            var rangeOfInts = Enumerable.Range(ints.Min(), ints.Max() - ints.Min() + 1);
            
            var intsNotInList = rangeOfInts.Except(ints).DefaultIfEmpty().ToList();
            
            var distinctInt = intsNotInList[new Random().Next(0, intsNotInList.Count() - 1)];
            fixture.Inject(distinctInt);
        }
    }
}
