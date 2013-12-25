using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;
using CyclicalSkipListTests.TestUtilities;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistDeleterTests
    {
        private const int MinimumGapSize = 2;
        private const int MaximumGapSize = 2*MinimumGapSize;

        public SkiplistDeleterTests()
        {
            SkiplistFactory.MinimumGapSize = MinimumGapSize;
        }

        [Theory]
        [FixedLengthSkiplistData(length: 0)]
        public void Delete_ingAKeyFromAnEmptySkiplist_ShouldReturnFalse
            (Skiplist<int> sut, int key)
        {
            // Fixture setup

            // Exercise system
            var result = sut.Delete(key);

            // Verify outcome
            var failureString =
                String.Format(
                "Removing key {0} from skiplist \n\n{1}\n returned true despite the skiplist being empty", 
                key,
                sut);

            Assert.False(result, failureString);

            // Teardown
        }

        [Theory]
        [FixedLengthSkiplistData(length: 1)]
        public void Delete_ingTheKeyOfTheHeadOfASingleNodeSkiplist_ShouldLeaveSkiplistEmpty
            (Skiplist<int> sut)
        {
            // Fixture setup
            var key = sut.Head.Key;

            // Exercise system
            sut.Delete(key);

            // Verify outcome
            var result = sut.Head == null;

            var failureString =
                String.Format(
                "Removing key {0} from single-node skiplist \n\n{1}\n left the head intact",
                key,
                sut);

            Assert.True(result, failureString);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 1)]
        public void Delete_ingTheKeyOfTheHeadOfASingleLevelSkiplist_ShouldDeleteOnlyOneNode
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = keys.Count - 1;

            var key = sut.Head.Key;

            // Exercise system
            sut.Delete(key);

            // Verify outcome
            var result = sut.Head.Bottom().EnumerateRight().Count();

            var failureString =
                String.Format(
                "Removing key {0} from skiplist \n\n{1}\n left other than one node intact",
                key,
                sut);

            Assert.True(result == expectedResult, failureString);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 1)]
        public void Delete_ingAKeyFromASingleLevelSkiplistOtherThanTheHead_ShouldDeleteExactlyOneNode
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = keys.Count - 1;

            keys.Remove(sut.Head.Down.Key);
            var key = keys.First();

            // Exercise system
            sut.Delete(key);

            // Verify outcome
            var result = sut.Head.Bottom().EnumerateRight().Count();

            var failureString =
                String.Format(
                "Removing key {0} from skiplist \n\n{1}\n deleted other than exactly one node",
                key,
                sut);

            Assert.True(result == expectedResult, failureString);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 1)]
        public void Delete_ingAKeyFromASingleLevelSkiplistOtherThanTheHead_ShouldDeleteANodeContainingThatKey
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            keys.Remove(sut.Head.Down.Key);
            var key = keys.First();

            // Exercise system
            sut.Delete(key);

            // Verify outcome
            var result = sut.Head.Bottom().EnumerateRight().Any(node => node.Key == key);

            var failureString =
                String.Format(
                "Removing key {0} from skiplist \n\n{1}\n did not Delete the node containing that key",
                key,
                sut);

            Assert.False(result, failureString);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 2)]
        public void Delete_ingAKeyFromTheBaseOfAColumn_ShouldUpdateTheColumnToPointAtTheNodeToTheRight
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = sut.Head.Bottom().Right;
            
            var key = keys.Min();

            // Exercise system
            sut.Delete(key);

            // Verify outcome
            var result = sut.Head.Bottom();

            var failureString =
                String.Format(
                "Removing key {0} from skiplist \n\n{1}\n did not update the column to point at node {2}",
                key,
                sut,
                expectedResult);

            Assert.True(result == expectedResult, failureString);

            // Teardown
        }

        [Theory]
        [FixedHeightSkiplistData(height: 2)]
        public void Delete_ingAKeyFromTheBaseOfAColumn_ShouldUpdateTheKeysInTheColumnToTheCorrectValue
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedKey = sut.Head.Bottom().Right.Key;

            var key = keys.Min();

            // Exercise system
            sut.Delete(key);

            // Verify outcome
            var result = sut.Head.EnumerateDown().Where(node => node.Key != expectedKey).ToList();

            var failureString =
                String.Format(
                "Removing key {0} from skiplist \n\n{1}\n did not update the keys in the column. Instead, nodes {2} remained",
                key,
                sut,
                String.Join(", ", result));

            Assert.False(result.Any(), failureString);

            // Teardown
        }

        [Theory]
        [FixedLengthSkiplistData(length: 2*MaximumGapSize)]
        public void Delete_ingAKeyFromMinimallySizedGapWithAMoreThanMinimalGapToTheRight_ShouldCauseTheGapToBorrowFromTheOneToTheRight
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = 2;

            var key = keys.Min();

            sut.Head.Down.Down.ConnectTo(sut.Head.Down.Right.Down.Left);

            // Exercise system
            sut.Delete(key);

            // Verify outcome
            var result = sut.Head.SizeOfGap();

            var failureString =
                String.Format(
                "Removing key {0} from skiplist \n\n{1}\n did not cause the left gap to borrow from the right gap",
                key,
                sut);

            Assert.True(result == expectedResult, failureString);

            // Teardown
        }

        [Theory]
        [FixedLengthSkiplistData(length: 2 * MaximumGapSize)]
        public void Delete_ingAKeyFromMinimallySizedGapWithMinimalGapToTheRight_ShouldCauseTheGapToMergeWithTheOneToTheRight
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = 3;

            var key = keys.Min();

            sut.Head.Down.Down.ConnectTo(sut.Head.Down.Right.Down.Left);
            sut.Head.Down.Right.Down.ConnectTo(sut.Head.Down.Right.Right.Down.Left);

            // Exercise system
            sut.Delete(key);

            // Verify outcome
            var result = sut.Head.SizeOfGap();

            var failureString =
                String.Format(
                "Removing key {0} from skiplist \n\n{1}\n did not cause the left gap to merge with the right gap",
                key,
                sut);

            Assert.True(result == expectedResult, failureString);

            // Teardown
        }

        [Theory]
        [FixedLengthSkiplistData(length: 2 * MaximumGapSize)]
        public void Delete_ingAKeyFromMinimallySizedGapWithNoGapToTheRight_ShouldCauseTheGapToMergeWithTheOneToTheLeft
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = 3;

            var key = keys.Max();

            foreach (var node in sut.Head.Down.EnumerateRight())
            {
                node.Down.ConnectTo(node.Right.Down.Left);
            }

            // Exercise system
            sut.Delete(key);

            // Verify outcome
            var result = sut.Head.Left.SizeOfGap();

            var failureString =
                String.Format(
                "Removing key {0} from skiplist \n\n{1}\n did not cause the right gap to merge with the left gap",
                key,
                sut);

            Assert.True(result == expectedResult, failureString);

            // Teardown
        }

        [Theory]
        [FixedLengthSkiplistData(length: 2 * MaximumGapSize)]
        public void Delete_ingAKeyFromAPairOfMinimallySizedGaps_ShouldCauseHeightOfTheSkiplistToDropByOne
            (Skiplist<int> sut, List<int> keys)
        {
            // Fixture setup
            var expectedResult = 1;

            var key = keys.Min();

            foreach (var node in sut.Head.Down.EnumerateRight())
            {
                node.Down.ConnectTo(node.Right.Down.Left);
            }

            // Exercise system
            sut.Delete(key);

            // Verify outcome
            var result = sut.Head.Height();

            var failureString =
                String.Format(
                "Removing key {0} from skiplist \n\n{1}\n did not cause the skiplist to drop in height by 1",
                key,
                sut);

            Assert.True(result == expectedResult, failureString);

            // Teardown
        }
    }
}
