using System;
using Ploeh.AutoFixture.Xunit;

namespace CyclicalSkipListTests.TestUtilities
{
    public class AutoRandomRepeatCountDataAttribute : AutoDataAttribute
    {
        public AutoRandomRepeatCountDataAttribute(int minimum, int maximum)
        {
            Fixture.RepeatCount = new Random().Next(minimum, maximum);
        }
    }
}
