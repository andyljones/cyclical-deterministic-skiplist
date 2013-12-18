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
    public class SkiplsistRemoverTests
    {
        private const int MinimumLength = 10;
        private const int MaximumLength = 20;

        private const int MinimumGapSize = 2;
        private const int MaximumGapSize = 4;

        private const int Timeout = 5000;

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

        [Theory(Timeout = Timeout)]
        [AutoSkiplistData(3, MaximumGapSize)]
        public void Remove_ingAKeyFromGapLargerThan2_ShouldRemoveTheCorrespondingNodeFromTheBottomLayer
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

        [Theory(Timeout = Timeout)]
        [AutoSkiplistData(3, MaximumGapSize)]
        public void Remove_ingTheLastKeyInAGapLargerThan2_ShouldPreserveTheConnectionsBetweenLayers
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

        [Theory(Timeout = Timeout)]
        [AutoSkiplistData(2*MaximumGapSize, 2*MaximumGapSize)]
        public void Remove_ingAKeyFromASize2GapWithASize3orMoreGapToTheRight_ShouldPreserveMinimumGapSize
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup - reduce first gap to size 2:
            var sizeOfFirstGap = sut.Head.Down.SizeOfGap();
            var firstNodeInGap = sut.Head.Bottom();
            var lastNodeInGap = sut.Head.Bottom().RightBy(sizeOfFirstGap-1);
            firstNodeInGap.ConnectTo(lastNodeInGap);

            var keyToBeRemoved = keys.Min();

            // Exercise system
            sut.Remove(keyToBeRemoved);

            // Verify outcome
            var results = SkiplistUtilities.EnumerateNodesInLevel(sut.Head.Down)
                                           .Select(node => node.SizeOfGap())
                                           .ToList();

            Assert.True(results.All(gapSize => gapSize >= MinimumGapSize));
            Assert.True(results.All(gapSize => gapSize <= MaximumGapSize));

            // Teardown
        }

        [Theory(Timeout = Timeout)]
        [AutoSkiplistData(2 * MaximumGapSize, 2 * MaximumGapSize)]
        public void Remove_ingAKeyFromASize2GapWithASize2GapToTheRight_ShouldPreserveMinimumGapSize
            (Skiplist<int> sut, List<int> keys)
        {
            //Fixture setup - reduce first & second gap to size 2:
            var sizeOfFirstGap = sut.Head.Down.SizeOfGap();
            var firstNodeInFirstGap = sut.Head.Bottom();
            var lastNodeInFirstGap = firstNodeInFirstGap.RightBy(sizeOfFirstGap - 1);
            firstNodeInFirstGap.ConnectTo(lastNodeInFirstGap);

            var sizeOfLastGap = sut.Head.Down.Right.SizeOfGap();
            var firstNodeInSecondGap = sut.Head.Down.Right.Down;
            var lastNodeInSecondGap = firstNodeInSecondGap.RightBy(sizeOfLastGap - 1);
            firstNodeInSecondGap.ConnectTo(lastNodeInSecondGap);

            var keyToBeRemoved = keys.Min();

            // Exercise system
            sut.Remove(keyToBeRemoved);

            // Verify outcome
            var results = SkiplistUtilities.EnumerateNodesInLevel(sut.Head)
                                           .Select(node => node.SizeOfGap())
                                           .ToList();

            Assert.True(results.All(gapSize => gapSize >= MinimumGapSize));
            Assert.True(results.All(gapSize => gapSize <= MaximumGapSize));

            // Teardown
        }

        [Theory(Timeout = Timeout)]
        [AutoSkiplistData(2 * MaximumGapSize, 2 * MaximumGapSize)]
        public void Remove_ingAKeyFromASize2GapWithNoGapToTheRightAndASize3orMoreGapToTheLeft_ShouldPreserveMinimumGapSize
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup - reduce first gap to size 2:
            var sizeOfFirstGap = sut.Head.Down.SizeOfGap();
            var firstNodeInGap = sut.Head.Bottom();
            var lastNodeInGap = sut.Head.Bottom().RightBy(sizeOfFirstGap - 1);
            firstNodeInGap.ConnectTo(lastNodeInGap);

            var keyToBeRemoved = keys.Max();

            // Exercise system
            sut.Remove(keyToBeRemoved);

            // Verify outcome
            var results = SkiplistUtilities.EnumerateNodesInLevel(sut.Head.Down)
                                           .Select(node => node.SizeOfGap())
                                           .ToList();

            Assert.True(results.All(gapSize => gapSize >= MinimumGapSize));
            Assert.True(results.All(gapSize => gapSize <= MaximumGapSize));

            // Teardown
        }

        [Theory(Timeout = Timeout)]
        [AutoSkiplistData(2 * MaximumGapSize, 2 * MaximumGapSize)]
        public void Remove_ingAKeyFromASize2GapWithNoGapToTheRightAndASize2GapToTheLeft_ShouldPreserveMinimumGapSize
            (Skiplist<int> sut, List<int> keys)
        {
            //Fixture setup - reduce first & second gap to size 2:
            var sizeOfFirstGap = sut.Head.Down.SizeOfGap();
            var firstNodeInFirstGap = sut.Head.Bottom();
            var lastNodeInFirstGap = firstNodeInFirstGap.RightBy(sizeOfFirstGap - 1);
            firstNodeInFirstGap.ConnectTo(lastNodeInFirstGap);

            var sizeOfLastGap = sut.Head.Down.Right.SizeOfGap();
            var firstNodeInSecondGap = sut.Head.Down.Right.Down;
            var lastNodeInSecondGap = firstNodeInSecondGap.RightBy(sizeOfLastGap - 1);
            firstNodeInSecondGap.ConnectTo(lastNodeInSecondGap);

            var keyToBeRemoved = keys.Max();

            // Exercise system
            sut.Remove(keyToBeRemoved);

            // Verify outcome
            var results = SkiplistUtilities.EnumerateNodesInLevel(sut.Head)
                                           .Select(node => node.SizeOfGap())
                                           .ToList();

            Assert.True(results.All(gapSize => gapSize >= MinimumGapSize));
            Assert.True(results.All(gapSize => gapSize <= MaximumGapSize));

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(2, 2)]
        public void Remove_ingANodeFromA2ElementSkiplist_ShouldReduceTheHeightOfTheSkiplist
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var keyToBeRemoved = keys[new Random().Next(0, keys.Count - 1)];

            // Exercise system
            sut.Remove(keyToBeRemoved);

            // Verify outcome
            Assert.Null(sut.Head.Down);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(4*MaximumGapSize+1, 5*MaximumGapSize)]
        public void Remove_ingAUniqueKey_ShouldRemoveItFromAllNodesInTheSkiplist
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var keyToBeRemoved = keys.Max();

            // Exercise system
            sut.Remove(keyToBeRemoved);

            // Verify outcome
            var levels = SkiplistUtilities.EnumerateLevels(sut.Head);
            var nodes = levels.SelectMany(SkiplistUtilities.EnumerateNodesInLevel);
            var results = nodes.Where(node => node.Key == keyToBeRemoved);

            Assert.Empty(results);

            // Teardown
        }
    }
}
