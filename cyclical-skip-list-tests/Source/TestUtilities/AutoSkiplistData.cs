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
        public AutoSkiplistDataAttribute(int minimumLength, int maximumLength)
        {
            var numberOfKeys = new Random().Next(minimumLength, maximumLength);
            var keys = Fixture.CreateMany<int>(numberOfKeys);
            Fixture.Inject(keys.ToList());

            Fixture.Register(() => SkiplistFactory.CreateFrom(Fixture.Create<List<int>>()));
        }
    }
}
