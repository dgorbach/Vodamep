using System;
using System.Linq;
using Vodamep.Data.Dummy;
using Xunit;

namespace Vodamep.Hkpv.Model.Tests
{

    public class HkpvReportTests
    {

        public HkpvReportTests()
        {
            this.Report = DataGenerator.Instance.CreateHkpvReport(null, null, 0, 0, false);
        }

        protected HkpvReport Report { get; }

        [Fact]
        public void AsSorted_ActivitiesArSortedByType()
        {
            this.Report.AddDummyActivity("15,02,05");
            
            var sorted = this.Report.AsSorted();

            var expected = new[] { ActivityType.Lv02, ActivityType.Lv05, ActivityType.Lv15 };
            var result = sorted.Activities[0].Entries.ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsSorted_ActivitiesArSortedByDate()
        {
            var date1 = DateTime.Today.AddDays(-2);
            var date2 = DateTime.Today.AddDays(-1);
            var date3 = DateTime.Today;

            this.Report.AddDummyActivity("01", date2);
            this.Report.AddDummyActivity("02", date1);
            this.Report.AddDummyActivity("03", date3);


            var sorted = this.Report.AsSorted();

            Assert.Equal(new[] { date1, date2, date3 }, sorted.Activities.Select(x => x.DateD));

        }


        [Fact]
        public void AsSorted_ActivitiesArSortedByPersonId()
        {
            this.Report.AddDummyPersons(3);
            this.Report.AddDummyStaff();

            var p1 = this.Report.Persons[0].Id;
            var p2 = this.Report.Persons[1].Id;
            var p3 = this.Report.Persons[2].Id;

            var a1 = new Activity() { PersonId = p1, DateD = DateTime.Today, StaffId = this.Report.Staffs[0].Id };
            a1.Entries.Add(ActivityType.Lv01);
            var a2 = new Activity() { PersonId = p2, DateD = DateTime.Today, StaffId = this.Report.Staffs[0].Id };
            a2.Entries.Add(ActivityType.Lv01);
            var a3 = new Activity() { PersonId = p3, DateD = DateTime.Today, StaffId = this.Report.Staffs[0].Id };
            a3.Entries.Add(ActivityType.Lv01);

            this.Report.Activities.AddRange(new[] { a2, a3, a1 });

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

            var a1 = new Activity() { StaffId = s1, DateD = DateTime.Today, PersonId = this.Report.Persons[0].Id };
            a1.Entries.Add(ActivityType.Lv01);
            var a2 = new Activity() { StaffId = s2, DateD = DateTime.Today, PersonId = this.Report.Persons[0].Id };
            a2.Entries.Add(ActivityType.Lv01);
            var a3 = new Activity() { StaffId = s3, DateD = DateTime.Today, PersonId = this.Report.Persons[0].Id };
            a3.Entries.Add(ActivityType.Lv01);

            this.Report.Activities.AddRange(new[] { a2, a3, a1 });

            var sorted = this.Report.AsSorted();

            Assert.Equal(new[] { s1, s2, s3 }, sorted.Activities.Select(x => x.StaffId));
        }


        [Fact]
        public void WriteThenRead_ReportsAreEqual()
        {
            var report = HkpvReport.CreateDummyData();

            using (var s = report.WriteToStream())
            {
                var report2 = HkpvReport.Read(s);

                Assert.Equal(report, report2);
            }
        }
    }
}
