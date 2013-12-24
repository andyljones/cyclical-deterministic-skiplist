using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;
using CyclicalSkipListTests.TestUtilities;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class SkiplistValidatorTests
    {


        [Theory]
        [AutoSkiplistData(height: 3)]
        public void ReachableNodes_OnACorrectlyStructuredSkiplist_ShouldReturnCorrectNumberOfNodes
            (Skiplist<int> sut, List<int> anonymousKeys)
        {
            // Fixture setup
            var numberOfNodesOnBottom = anonymousKeys.Count;
            var numberOfNodesInMidlevel = (int)Math.Ceiling(numberOfNodesOnBottom / (double)SkiplistFactory.MaximumGapSize);
            var numberOfNodesAtTop = (int) Math.Ceiling(numberOfNodesInMidlevel/(double) SkiplistFactory.MaximumGapSize);
            var expectedResult = numberOfNodesOnBottom + numberOfNodesInMidlevel + numberOfNodesAtTop;

            // Exercise system
            var anonymousNodes = sut.ReachableNodes();

            // Verify outcome
            var result = anonymousNodes.Count();

            var failureString = String.Format(
                    "Different number ({0}) of nodes retrieved to number of nodes expected ({1}) in skiplist\n\n {2}", 
                    result,
                    expectedResult,
                    sut);

            Assert.True(expectedResult == result, failureString);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(height: 3)]
        public void ValidateRightwardsReachability_OnACorrectlyStructuredSkiplist_ShouldReturnTrue
            (Skiplist<int> sut)
        {
            // Fixture setup

            // Exercise system
            IEnumerable<INode<int>> anonymousOutput;
            var result = sut.ValidateRightwardsReachability(out anonymousOutput);

            // Verify outcome
            var failureString = String.Format(
                "The nodes reported as unreachable are \n {0} \n in skiplist {1}",
                String.Join(", ", anonymousOutput.Select(node => node.ToString())),
                sut);

            Assert.True(result, failureString);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(height: 3)]
        public void ValidateRightwardsReachability_WhenANodeIsOmittedFromALevel_ShouldFailAndReturnThatNode
            (Skiplist<int> sut)
        {
            // Fixture setup
            var removedNode = sut.Head.Right;
            removedNode.Left.ConnectTo(removedNode.Right);

            // Exercise system
            IEnumerable<INode<int>> output;
            var result = sut.ValidateRightwardsReachability(out output);

            output = output.ToList();

            // Verify outcome
            var failureString0 = String.Format(
                "Node {0} \n should not be reachable from the head of skiplist \n\n {1} \n but it was found anyway",
                removedNode,
                sut);
            Assert.False(result, failureString0);

            var failureString1 = String.Format(
                "Node {0} \n should not be reachable from the head of skiplist \n\n {1} \n but nodes \n {2} \n were returned",
                removedNode,
                sut,
                String.Join(", ", output.Select(node => node.ToString())));
            Assert.True(output.Count() == 1, failureString1);
            Assert.True(output.Contains(removedNode), failureString1);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(height: 3)]
        public void ValidateOrdering_OnACorrectlyStructuredSkiplist_ShouldReturnTrue
            (Skiplist<int> sut)
        {
            // Fixture setup

            // Exercise system
            IEnumerable<INode<int>> anonymousOutput;
            var result = sut.ValidateOrdering(out anonymousOutput);

            // Verify outcome
            var failureString = String.Format(
                "The nodes reported as out-of-order are \n {0} \n in skiplist {1}",
                String.Join(", ", anonymousOutput.Select(node => node.ToString())),
                sut);
            Assert.True(result, failureString);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(height: 3)]
        public void ValidateOrdering_WhenTwoKeysAreInterchanged_ShouldFailAndReturnTheConcernedNodes
            (Skiplist<int> sut)
        {
            // Fixture setup
            var temp = sut.Head.Down.Key;
            sut.Head.Down.Key = sut.Head.Down.Right.Key;
            sut.Head.Down.Right.Key = temp;

            // Exercise system
            IEnumerable<INode<int>> output;
            var result = sut.ValidateOrdering(out output);

            output = output.ToList();

            // Verify outcome
            var failureString0 = String.Format(
                "Nodes \n {0}, {1} \n are interchanged in skiplist \n\n {2} \n but skiplist is still considered valid",
                sut.Head.Down,
                sut.Head.Down.Right,
                sut);
            Assert.False(result, failureString0);

            var failureString1 = String.Format(
                "Nodes \n {0}, {1} \n are interchanged in skiplist \n\n {2} \n but nodes \n {3} \n were returned as erroneous instead",
                sut.Head.Down,
                sut.Head.Down.Right,
                sut,
                String.Join(", ", output.Select(node => node.ToString())));
            Assert.True(output.Count() <= 4, failureString1);
            Assert.True(
                output.Contains(sut.Head.Down) && 
                output.Contains(sut.Head.Down.Right), failureString1);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(height: 3)]
        public void ValidateGapSize_OnACorrectlyStructuredSkiplist_ShouldReturnTrue
            (Skiplist<int> sut)
        {
            // Fixture setup

            // Exercise system
            IEnumerable<INode<int>> anonymousOutput;
            var result = sut.ValidateGapSize(out anonymousOutput);

            // Verify outcome
            var failureString = String.Format(
                "The nodes reported as having invalid gapsizes are \n {0} \n in skiplist {1}",
                String.Join(", ", anonymousOutput.Select(node => node.ToString())),
                sut);
            Assert.True(result, failureString);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(height: 3)]
        public void ValidateGapSize_WhenAGapIsShrunkBelowMinimum_ShouldFailAndReturnThoseNodes
            (Skiplist<int> sut)
        {
            // Fixture setup
            sut.Head.Down.ConnectTo(sut.Head.Right.Down);

            // Exercise system
            IEnumerable<INode<int>> output;
            var result = sut.ValidateGapSize(out output);

            output = output.ToList();

            // Verify outcome
            var failureString0 = String.Format(
                "Node \n {0} \n has a size-1 gap in skiplist \n\n {1} \n but skiplist is still considered valid",
                sut.Head.Down,
                sut);
            Assert.False(result, failureString0);

            var failureString1 = String.Format(
                "Node \n {0} \n has a size-1 gap in skiplist \n\n {1} \n but nodes \n {2} \n were returned as erroneous instead",
                sut.Head,
                sut,
                String.Join(", ", output.Select(node => node.ToString())));
            Assert.True(output.Count() == 2, failureString1);
            Assert.True(output.Contains(sut.Head), failureString1);

            // Teardown
        }

        [Theory]
        [AutoSkiplistData(height: 3)]
        public void ValidateGapSize_WhenAGapIsExpandedAboveMaximum_ShouldFailAndReturnThoseNodes
            (Skiplist<int> sut)
        {
            // Fixture setup
            sut.Head.ConnectTo(sut.Head.Right.Right);

            // Exercise system
            IEnumerable<INode<int>> output;
            var result = sut.ValidateGapSize(out output);

            output = output.ToList();

            // Verify outcome
            var failureString0 = String.Format(
                "Node \n {0} \n has a too large gap in skiplist \n\n {1} \n but skiplist is still considered valid",
                sut.Head.Down,
                sut);
            Assert.False(result, failureString0);

            var failureString1 = String.Format(
                "Node \n {0} \n has a too-large gap in skiplist \n\n {1} \n but nodes \n {2} \n were returned as erroneous instead",
                sut.Head,
                sut,
                String.Join(", ", output.Select(node => node.ToString())));
            Assert.True(output.Count() == 1, failureString1);
            Assert.True(output.Contains(sut.Head), failureString1);

            // Teardown
        }
    }
}
