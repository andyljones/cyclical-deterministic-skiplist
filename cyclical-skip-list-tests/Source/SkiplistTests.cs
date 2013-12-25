using System;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;
using CyclicalSkipListTests.TestUtilities;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistTests
    {
        [Theory]
        [FixedHeightSkiplistData(height: 0)]
        public void Count_IfTheSkiplistIsEmpty_ShouldReturn0
            (Skiplist<int> sut)
        {
            // Fixture setup
            var expectedResult = 0;

            // Exercise system
            var result = sut.Count;

            // Verify outcome
            Assert.Equal(result, expectedResult);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 1)]
        public void Count_OnANonemptySkiplist_ShouldReturnTheNumberOfNodesInTheBottomLevelOfTheSkiplist
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = keys.Count;

            // Exercise system
            var result = sut.Count;

            // Verify outcome
            Assert.Equal(result, expectedResult);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 0)]
        public void GetEnumeratorT_IfTheSkiplistIsEmpty_ShouldReturnNothing
            (Skiplist<int> sut)
        {
            // Fixture setup

            // Exercise system
            var result = sut.GetEnumerator().MoveNext();

            // Verify outcome
            Assert.False(result);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 1)]
        public void GetEnumerator_OnANonemptySkiplist_ShouldReturnAllKeys
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = keys.OrderBy(x => x).ToList();

            // Exercise system
            var result = sut.GetEnumerator();

            // Verify outcome
            var i = 0;
            while (result.MoveNext())
            {
                Assert.Equal(expectedResult[i], result.Current);
                i++;
            }
            Assert.Equal(i, keys.Count);
            Assert.False(result.MoveNext());

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 1)]
        public void CopyTo_GivenASufficienlySizedArrayAndAnIndexOf2_ShouldCopyTheKeysInTheSkiplistToTheArrayInOrderFromIndex2
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = new[] {0, 0, 0}.Concat(keys.OrderBy(x => x)).ToArray();

            // Exercise system
            var result = new int[expectedResult.Count()];
            sut.CopyTo(result, 3);

            // Verify outcome
            Assert.Equal(expectedResult, result);

            // Teardown
        }
    }
}
