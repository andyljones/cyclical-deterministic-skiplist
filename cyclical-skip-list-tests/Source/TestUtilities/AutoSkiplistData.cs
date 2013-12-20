using System;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;

namespace CyclicalSkipListTests.TestUtilities
{
    public class AutoSkiplistDataAttribute : AutoDataAttribute
    {
        public AutoSkiplistDataAttribute(double minimumNumberOfGaps, double maximumNumberOfGaps)
        {
            var gapSize = SkiplistFactory.MaximumGapSize;
            var minimumLength = (int)(gapSize*minimumNumberOfGaps);
            var maximumLength = (int)(gapSize*maximumNumberOfGaps);

            var numberOfKeys = new Random().Next(minimumLength, maximumLength);

            var keys = new List<int>();
            Fixture.AddManyTo(keys, numberOfKeys);
            
            Fixture.Inject(keys);
            Fixture.Register(() => SkiplistFactory.CreateFrom(keys));
        }
    }
}
