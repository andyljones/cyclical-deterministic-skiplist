using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistTests
    {
        private const int MinimumLength = 10;
        private const int MaximumLength = 20;

        private const int MinimumGapSize = 2;
        private const int MaximumGapSize = 4;

        [Theory]
        [AutoSkiplistData(MinimumLength, MaximumLength)]
        public void Contains_WhenTheSkiplistContainsTheGivenKey_ShouldReturnTrue
            (Skiplist<int> sut, IList<int> keys)
        {
            // Fixture setup
            var key = keys[new Random().Next(0, keys.Count - 1)];

            // Exercise system
            var result = sut.Contains(key);

            // Verify outcome
            Assert.True(result);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MinimumLength, MaximumLength)]
        public void Contains_WhenTheSkiplistDoesNotContainAKeyWhichIsWithinTheRangeOfItsKeys_ShouldReturnFalse
            (Skiplist<int> sut, IList<int> keys, int distinctKey)
        {
            // Fixture setup

            // Exercise system
            var result = sut.Contains(distinctKey);

            // Verify outcome
            Assert.False(result);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MinimumLength, MaximumLength)]
        public void Contains_WhenTheSkiplistDoesNotContainAKeyLargerThanItsKeys_ShouldReturnFalse
            (Skiplist<int> sut, IList<int> keys)
        {
            // Fixture setup
            var key = keys.Max() + 1;

            // Exercise system
            var result = sut.Contains(key);

            // Verify outcome
            Assert.False(result);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MinimumLength, MaximumLength)]
        public void Insert_ingAKeyIntoTheSkiplist_ShouldInsertItInTheCorrectPosition
            (Skiplist<int> sut, IList<int> keys, int distinctKey)
        {
            // Fixture setup
            keys.Add(distinctKey);
            var expectedResults = keys.OrderBy(item => item);

            // Exercise system
            sut.Insert(distinctKey);

            // Verify outcome
            var results = SkiplistUtilities.EnumerateKeysInLevel(sut.Head.Bottom());
            
            Assert.Equal(expectedResults, results);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MinimumLength, MaximumLength)]
        public void Insert_ingAKeyMultipleTimes_ShouldPreserveMaximumGapsize
            (Skiplist<int> sut, int distinctKey)
        {
            // Fixture setup

            // Exercise system
            for (int i = 0; i < MaximumGapSize+1; i++)
            {
                sut.Insert(distinctKey);                
            }

            // Verify outcome
            var levels = SkiplistUtilities.EnumerateLevels(sut.Head).Reverse().Skip(1);
            var nodes = levels.SelectMany(SkiplistUtilities.EnumerateNodesInLevel);
            var gaps = nodes.Select(node => node.SizeOfGap()).ToList();

            Assert.True(gaps.All(gap => gap <= MaximumGapSize));
            Assert.True(gaps.All(gap => gap >= MinimumGapSize));

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(MinimumLength, MaximumLength)]
        public void Insert_ingAKeyMultipleTimes_ShouldLeaveSkiplistProperlyConnected
            (Skiplist<int> sut, int distinctKey)
        {
            // Fixture setup

            // Exercise system
            for (int i = 0; i < MaximumGapSize + 1; i++)
            {
                sut.Insert(distinctKey);
            }

            // Verify outcome
            var levels = SkiplistUtilities.EnumerateLevels(sut.Head).Reverse().Skip(1);
            var nodes = levels.SelectMany(SkiplistUtilities.EnumerateNodesInLevel);

            Assert.True(nodes.All(node => node.Right.Left == node));

            // Teardown
        }

        [Theory]
        [AutoData]
        public void Insert_ingTwoNodesIntoAnEmptySkiplist_ShouldCreateATopLevelWithOneNode
            (int key)            
        {
            // Fixture setup
            var sut = new Skiplist<int>();

            // Exercise system
            sut.Insert(key);
            sut.Insert(key);

            // Verify outcome
            Assert.Equal(sut.Head, sut.Head.Right);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(1, MaximumGapSize)]
        public void Insert_ingAKeyIntoTheSkiplist_ShouldLeaveTheBottomLevelProperlyConnected
            (Skiplist<int> sut, int distinctKey)
        {
            // Fixture setup
            
            // Exercise system
            sut.Insert(distinctKey);

            // Verify outcome
            var nodes = SkiplistUtilities.EnumerateNodesInLevel(sut.Head.Bottom()).ToList();

            Assert.True(nodes.All(node => node.Right.Left == node));

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(3, MaximumGapSize)]
        public void Remove_ingAKeyFromASize2To4Gap_ShouldRemoveTheCorrespondingNodeFromTheBottomLayer
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var keyToBeRemoved = keys[new Random().Next(0, keys.Count - 1)];

            keys.Remove(keyToBeRemoved);
            var expectedResults = keys.OrderBy(key => key);
            
            // Exercise system
            sut.Remove(keyToBeRemoved);

            // Verify outcome
            var results = SkiplistUtilities.EnumerateKeysInLevel(sut.Head.Bottom());

            Assert.Equal(expectedResults, results);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(1, MaximumGapSize)]
        public void Remove_ingTheFirstKeyInASkiplist_ShouldPreserveTheConnectionsBetweenLayers
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var keyToBeRemoved = keys.OrderBy(key => key).Last();

            // Exercise system
            sut.Remove(keyToBeRemoved);

            // Verify outcome
            Assert.NotNull(sut.Head.Down);

            // Teardown
        }
    }
}
