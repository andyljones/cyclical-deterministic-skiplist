using System;
using System.Collections.Generic;
using System.Linq;
using CyclicalSkipList;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;

namespace CyclicalSkipListTests.TestUtilities
{
    public class FixedLengthSkiplistDataAttribute : AutoDataAttribute
    {
        public FixedLengthSkiplistDataAttribute(int length)
        {
            var keys = new List<int>();
            Fixture.AddManyTo(keys, length);

            Fixture.Inject(keys);
            Fixture.Register(() => SkiplistFactory.CreateFrom(keys));
        }
    }
}
