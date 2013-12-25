using System;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;

namespace CyclicalSkipListTests.TestUtilities
{
    public class FixedHeightSkiplistDataAttribute : AutoDataAttribute
    {
        public FixedHeightSkiplistDataAttribute(int height)
        {
            int numberOfKeys;
            if (height != 0)
            {
                var gapSize = SkiplistFactory.MaximumGapSize;
                var minimumLength = (int) Math.Ceiling(Math.Pow(gapSize, height - 1) + 1);
                var maximumLength = (int) Math.Floor(Math.Pow(gapSize, height));
                numberOfKeys = new Random().Next(minimumLength, maximumLength);
            }
            else
            {
                numberOfKeys = 0;
            }

            var keys = new List<int>();
            Fixture.AddManyTo(keys, numberOfKeys);
            
            Fixture.Inject(keys);
            Fixture.Register(() => SkiplistFactory.CreateFrom(keys));
        }
    }
}
