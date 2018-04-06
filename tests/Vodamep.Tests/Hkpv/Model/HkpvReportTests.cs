using System.Linq;
using Vodamep.Data.Dummy;
using Vodamep.Hkpv.Validation.Tests;
using Vodamep.Model;
using Xunit;

namespace Vodamep.Hkpv.Model.Tests
{
    public class HkpvReportTests : ValidationTestsBase
    {
        [Fact]
        public void AsSorted_ActivitiesArSortedByType()
        {
            this.Report.AddDummyActivity("15");
            this.Report.AddDummyActivity("02");
            this.Report.AddDummyActivity("05");

            var sorted = this.Report.AsSorted();

            Assert.Equal(new[] { 2, 5, 15 }, sorted.Activities.Select(x => (int)x.Type));

        }

        [Fact]
        public void AsSorted_ActivitiesArSortedByDate()
        {
            var date1 = LocalDate.Today.AddDays(-2);
            var date2 = LocalDate.Today.AddDays(-1);
            var date3 = LocalDate.Today;

            this.Report.AddDummyActivity("01", date2);
            this.Report.AddDummyActivity("02", date1);
            this.Report.AddDummyActivity("03", date3);


            var sorted = this.Report.AsSorted();

            Assert.Equal(new[] { date1, date2, date3 }, sorted.Activities.Select(x => x.Date));

        }


        [Fact]
        public void AsSorted_ActivitiesArSortedByPersonId()
        {
            this.Report.AddDummyPersons(3);
            this.Report.AddDummyStaff();

            var p1 = this.Report.Persons[0].Id;
            var p2 = this.Report.Persons[1].Id;
            var p3 = this.Report.Persons[2].Id;

            var a1 = new Activity() { PersonId = p1, Date = LocalDate.Today, Amount = 1, StaffId = this.Report.Staffs[0].Id, Type = ActivityType.Lv01 };
            var a2 = new Activity() { PersonId = p2, Date = LocalDate.Today, Amount = 1, StaffId = this.Report.Staffs[0].Id, Type = ActivityType.Lv01 };
            var a3 = new Activity() { PersonId = p3, Date = LocalDate.Today, Amount = 1, StaffId = this.Report.Staffs[0].Id, Type = ActivityType.Lv01 };

            this.Report.Activities.AddRange(new [] { a2, a3, a1 });

            var sorted = this.Report.AsSorted();

            Assert.Equal(new[] { p1, p2, p3 }, sorted.Activities.Select(x => x.PersonId));
        }

        [Fact]
        public void AsSorted_ActivitiesArSortedByStaffId()
        {
            this.Report.AddDummyPerson();
            this.Report.AddDummyStaffs(3);

            var s1 = this.Report.Staffs[0].Id;
            var s2 = this.Report.Staffs[1].Id;
            var s3 = this.Report.Staffs[2].Id;

            var a1 = new Activity() { StaffId = s1, Date = LocalDate.Today, Amount = 1, PersonId = this.Report.Persons[0].Id, Type = ActivityType.Lv01 };
            var a2 = new Activity() { StaffId = s2, Date = LocalDate.Today, Amount = 1, PersonId = this.Report.Persons[0].Id, Type = ActivityType.Lv01 };
            var a3 = new Activity() { StaffId = s3, Date = LocalDate.Today, Amount = 1, PersonId = this.Report.Persons[0].Id, Type = ActivityType.Lv01 };

            this.Report.Activities.AddRange(new[] { a2, a3, a1 });

            var sorted = this.Report.AsSorted();

            Assert.Equal(new[] { s1, s2, s3 }, sorted.Activities.Select(x => x.StaffId));
        }
    }
}
