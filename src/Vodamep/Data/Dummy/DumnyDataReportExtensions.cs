using System;
using System.Linq;
using Vodamep.Hkpv.Model;


namespace Vodamep.Data.Dummy
{
    public static class DumnyDataReportExtensions
    {
        public static (Person person, PersonalData Data) AddDummyPerson(this HkpvReport report)
        {
            var p = DataGenerator.Instance.CreatePerson();
            report.AddPerson(p);
            return p;
        }

        public static (Person person, PersonalData Data)[] AddDummyPersons(this HkpvReport report, int count)
        {
            var p = DataGenerator.Instance.CreatePersons(count).ToArray();
            report.AddPersons(p);
            return p;
        }


        public static Staff AddDummyStaff(this HkpvReport report)
        {
            var s = DataGenerator.Instance.CreateStaff();

            report.Staffs.Add(s);
            return s;
        }

        public static Staff[] AddDummyStaffs(this HkpvReport report, int count)
        {
            var s = DataGenerator.Instance.CreateStaffs(count).ToArray();
            report.Staffs.AddRange(s);
            return s;
        }

        public static Activity[] AddDummyActivity(this HkpvReport report, string code, DateTime? date = null)
        {
            if (!report.Persons.Any())
                report.AddDummyPerson();

            if (!report.Staffs.Any())
                report.AddDummyStaff();

            var result = code.Split(',').GroupBy(x => x).Select(x => new Activity()
            {
                PersonId = report.Persons[0].Id,
                StaffId = report.Staffs[0].Id,
                DateD = date ?? report.FromD,
                Amount = x.Count(),
                Type = (ActivityType)int.Parse(x.Key)
            }).ToArray();

            report.Activities.AddRange(result);

            return result;
        }

        public static Consultation[] AddDummyConsultation(this HkpvReport report, string code, DateTime? date = null)
        {
            if (!report.Staffs.Any())
                report.AddDummyStaff();

            var result = code.Split(',').GroupBy(x => x).Select(x => new Consultation()
            {
                StaffId = report.Staffs[0].Id,
                DateD = date ?? DateTime.Today,
                Amount = x.Count(),
                Type = (ConsultationType)int.Parse(x.Key)
            }).ToArray();

            report.Consultations.AddRange(result);

            return result;
        }

        public static Activity[] AddDummyActivities(this HkpvReport report)
        {
            if (report.FromD == null)
                report.FromD = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);

            if (report.ToD == null)
                report.ToD = report.FromD.LastDateInMonth();

            if (report.Staffs.Count == 0)
                report.AddDummyStaff();

            if (report.Persons.Count == 0)
                report.AddDummyPerson();

            var a = DataGenerator.Instance.CreateActivities(report);
            report.Activities.AddRange(a);
            return a;
        }

    }
}
