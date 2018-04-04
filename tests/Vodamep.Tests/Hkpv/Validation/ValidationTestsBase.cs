using FluentValidation;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Vodamep.Hkpv.Model;
using Vodamep.Model;
using Vodamep.TestData;
using Xunit;

namespace Vodamep.Hkpv.Validation.Tests
{
    public abstract class ValidationTestsBase
    {
        public ValidationTestsBase()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

            this.DataGenerator = new TestDataGenerator();
            this.Report = new HkpvReport()
            {
                Institution = new Institution() { Id = "1", Name = "Testverein" }
            };
            this.Report.From = LocalDate.Today.AddMonths(-1).FirstDateInMonth();
            this.Report.To = this.Report.From.LastDateInMonth();                       

            var loc = new DisplayNameResolver();

            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
        }

        protected TestDataGenerator DataGenerator { get; }
        protected HkpvReport Report { get; }

        protected (Person person, PersonalData Data) AddPerson()
        {
            var p = this.DataGenerator.CreatePerson();
            Report.AddPerson(p);
            return p;
        }

        protected (Person person, PersonalData Data)[] AddPersons(int count)
        {
            var p = this.DataGenerator.CreatePersons(count).ToArray();
            Report.AddPersons(p);
            return p;
        }


        protected Staff AddStaff()
        {
            var s = this.DataGenerator.CreateStaff();
            Report.Staffs.Add(s);
            return s;
        }

        protected Staff[] AddStaffs(int count)
        {
            var s = this.DataGenerator.CreateStaffs(count).ToArray();
            Report.Staffs.AddRange(s);
            return s;
        }

        protected Activity[] AddActivity(string code, LocalDate date = null)
        {
            if (!this.Report.Persons.Any())
                this.AddPerson();

            if (!this.Report.Staffs.Any())
                this.AddStaff();

            var result = code.Split(',').GroupBy(x => x).Select(x => new Activity()
            {
                PersonId = this.Report.Persons[0].Id,
                StaffId = this.Report.Staffs[0].Id,
                Date = date ?? LocalDate.Today,
                Amount = x.Count(),
                Type = (ActivityType)int.Parse(x.Key)
            }).ToArray();

            this.Report.Activities.AddRange(result);

            return result;
        }
        
        protected Consultation[] AddConsultation(string code, LocalDate date = null)
        {
            if (!this.Report.Staffs.Any())
                this.AddStaff();

            var result = code.Split(',').GroupBy(x => x).Select(x => new Consultation()
            {                
                StaffId = this.Report.Staffs[0].Id,
                Date = date ?? LocalDate.Today,
                Amount = x.Count(),
                Type = (ConsultationType)int.Parse(x.Key)
            }).ToArray();

            this.Report.Consultations.AddRange(result);

            return result;
        }

        protected Activity[] AddActivities()
        {
            if (this.Report.From == null)
                this.Report.From = new Vodamep.Model.LocalDate(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);

            if (this.Report.To == null)
                this.Report.To = this.Report.From.LastDateInMonth();

            if (this.Report.Staffs.Count == 0)
                AddStaff();

            if (this.Report.Persons.Count == 0)
                AddPerson();

            var a = this.DataGenerator.CreateActivities(this.Report);
            Report.Activities.AddRange(a);
            return a;
        }

        protected void AssertError(string error)
        {
            var v = new HkpvReportValidator();
            var result = v.Validate(this.Report);

            Assert.Contains(error, result.Errors.Select(x => x.ErrorMessage));
        }

        protected void AssertErrorRegExp(string pattern)
        {
            var r = new Regex(pattern);

            var v = new HkpvReportValidator();
            var result = v.Validate(this.Report);

            Assert.True(result.Errors.Where(x => r.IsMatch(x.ErrorMessage)).Any());
        }
    }
}
