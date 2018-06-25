using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Vodamep.Hkpv.Model.Tests
{
    public class ActivityTests
    {
        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, 0)]
        [InlineData(new int[] { 1, 2 }, new int[] { 1 }, 1)]
        [InlineData(new int[] { 1 }, new int[] { 1, 2 }, -1)]
        [InlineData(new int[] { 2 }, new int[] { 1 }, 1)]
        [InlineData(new int[] { }, new int[] { }, 0)]
        [InlineData(new int[] { 1 }, new int[] { }, 1)]
        public void CompareTo_ComparesToListOfActivites_ReturnsTheExpectedDirection(int[] a1, int[] a2, int expectedDirection)
        {
            var activity1 = new Activity();
            activity1.Entries.AddRange(a1.Select(x => (ActivityType)x));

            var activity2 = new Activity();
            activity2.Entries.AddRange(a2.Select(x => (ActivityType)x));

            var c = activity1.CompareTo(activity2);

            var direction = c == 0 ? 0 : c / Math.Abs(c);

            Assert.Equal(direction, expectedDirection);
        }

        [Fact]
        public void AsSorted_ReturnspectedResult()
        {

            var list1 = new[] {
                new ActivityType[]{ },
                new []{ ActivityType.Lv01},
                new []{ ActivityType.Lv01,ActivityType.Lv02},
                new []{ ActivityType.Lv02},
            };


            var list2 = new List<Activity>();
            foreach ( var entry in list1)
            {
                var a = new Activity();
                a.Entries.AddRange(entry.OrderByDescending(x => x));
                list2.Insert(0, a);
            }

            var sortedList = list2.AsSorted().Select(x => x.Entries.ToArray()).ToArray();

            Assert.Equal(list1, sortedList);
        }
    }
}
