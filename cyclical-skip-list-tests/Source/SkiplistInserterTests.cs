using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistInserterTests
    {
        private const int MinimumLength = 10;
        private const int MaximumLength = 20;

        private const int MinimumGapSize = 2;
        private const int MaximumGapSize = 4;

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
            for (int i = 0; i < MaximumGapSize + 1; i++)
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
        [AutoRandomRepeatCountData(MaximumGapSize + 1, 2 * MaximumGapSize)]
        public void Insert_ingNodesIntoAnEmptySkiplist_ShouldCreateATopLevelWithOneNode
            (List<int> keys)
        {
            // Fixture setup
            var sut = new Skiplist<int>();

            // Exercise system
            foreach (var key in keys)
            {
                sut.Insert(key);
                Debug.WriteLine(key + "\n");
                Debug.WriteLine(sut + "\n");
            }

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

        //[Theory]
        //[AutoSkiplistData(10 * MaximumGapSize, 15 * MaximumGapSize)]
        //public void Insert_ingManyKeysWillProduceAValidSkiplistAfterEveryInsert
        //    (List<int> keys)
        //{
        //    // Fixture setup
        //    var sut = new Skiplist<int>();

        //    // Exercise system
        //    foreach (var key in keys)
        //    {
        //        sut.Insert(key);
        //        Debug.WriteLine(key + "\n");
        //        Debug.WriteLine(sut + "\n");
        //    }

        //    // Verify outcome
        //    var levels = SkiplistUtilities.EnumerateLevels(sut.Head).ToList();
        //    var nodes = levels.SelectMany(SkiplistUtilities.EnumerateNodesInLevel).ToList();

        //    var results0 = nodes.Where(node => node.Right.Left != node);
        //    var results1 = nodes.Where(node => node.Up != null).Where(node => node.Up.Down != node);
        //    var results2 = nodes.Except(levels).Where(node => node.Key < node.Left.Key);

        //    Assert.Empty(results0);
        //    Assert.Empty(results1);
        //    Assert.Empty(results2);
        //}
    }
}
