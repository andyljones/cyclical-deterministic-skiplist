using System;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace CyclicalSkipListTests
{
    public class CompareToCyclicOrdererAdapterTests
    {
        [Theory]
        [AutoData]
        public void CyclicCompare_GivenThreeElementsInLinearOrder_ShouldReturnTrue
            (List<int> elements)
        {
            // Fixture setup
            var sut = new CompareToCyclicOrdererAdapter<int>(Comparer<int>.Default.Compare);

            elements = elements.OrderBy(i => i).ToList();

            // Exercise system
            var result = sut.InOrder(elements[0], elements[1], elements[2]);

            // Verify outcome
            var failureString = String.Format(
                "Elements were {0}, {1}, {2}, but cyclic comparer thought them out of order", 
                elements.Select(i => i.ToString()).ToArray());

            Assert.True(result, failureString);

            // Teardown
        }

        [Theory]
        [AutoData]
        public void CyclicCompare_GivenThreeElementsInCycledLinearOrder_ShouldReturnTrue
            (List<int> elements)
        {
            // Fixture setup
            var sut = new CompareToCyclicOrdererAdapter<int>(Comparer<int>.Default.Compare);

            elements = elements.OrderBy(i => i).ToList();

            // Exercise system
            var result = sut.InOrder(elements[1], elements[2], elements[0]);

            // Verify outcome
            var failureString = String.Format(
                "Elements were {1}, {2}, {0}, but cyclic comparer thought them out of order",
                elements.Select(i => i.ToString()).ToArray());

            Assert.True(result, failureString);

            // Teardown
        }

        [Theory]
        [AutoData]
        public void CyclicCompare_GivenTwoMatchingElementsInTheFirstTwoPositions_ShouldReturnTrue
            (List<int> elements)
        {
            // Fixture setup
            var sut = new CompareToCyclicOrdererAdapter<int>(Comparer<int>.Default.Compare);

            elements = elements.OrderBy(i => i).ToList();

            // Exercise system
            var result = sut.InOrder(elements[0], elements[0], elements[2]);

            // Verify outcome
            var failureString = String.Format(
                "Elements were {0}, {0}, {2}, but cyclic comparer thought them out of order",
                elements.Select(i => i.ToString()).ToArray());

            Assert.True(result, failureString);

            // Teardown
        }

        [Theory]
        [AutoData]
        public void CyclicCompare_GivenTwoMatchingElementsInTheFirstAndLastPositions_ShouldReturnTrue
            (List<int> elements)
        {
            // Fixture setup
            var sut = new CompareToCyclicOrdererAdapter<int>(Comparer<int>.Default.Compare);

            elements = elements.OrderBy(i => i).ToList();

            // Exercise system
            var result = sut.InOrder(elements[0], elements[1], elements[0]);

            // Verify outcome
            var failureString = String.Format(
                "Elements were {0}, {1}, {0}, but cyclic comparer thought them out of order",
                elements.Select(i => i.ToString()).ToArray());

            Assert.True(result, failureString);

            // Teardown
        }

        [Theory]
        [AutoData]
        public void CyclicCompare_GivenThreeElementsNotInCyclicOrder_ShouldReturnFalse
            (List<int> elements)
        {
            // Fixture setup
            var sut = new CompareToCyclicOrdererAdapter<int>(Comparer<int>.Default.Compare);

            elements = elements.OrderBy(i => i).ToList();

            // Exercise system
            var result = sut.InOrder(elements[0], elements[2], elements[1]);

            // Verify outcome
            var failureString = String.Format(
                "Elements were {0}, {2}, {1}, but cyclic comparer thought them in order",
                elements.Select(i => i.ToString()).ToArray());

            Assert.False(result, failureString);

            // Teardown
        }
    }
}
