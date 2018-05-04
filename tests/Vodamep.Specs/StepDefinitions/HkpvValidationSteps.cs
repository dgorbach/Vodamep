using FluentValidation;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using TechTalk.SpecFlow;
using Vodamep.Data.Dummy;
using Vodamep.Hkpv.Model;
using Vodamep.Hkpv.Validation;
using Xunit;

namespace Vodamep.Specs.StepDefinitions
{

    [Binding]
    public class HkpvValidationSteps
    {

        private HkpvReportValidationResult _result;

        public HkpvValidationSteps()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

            var loc = new DisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            var date = DateTime.Today.AddMonths(-1);
            this.Report = DataGenerator.Instance.CreateHkpvReport(date.Year, date.Month, 1, 1, false);
            this.AddActivities(Report, Report.Persons[0].Id, Report.Staffs[0].Id);
            this.AddConsultation(Report, Report.Staffs[0].Id);
        }

        private void AddActivities(HkpvReport report, string personId, string staffId)
        {
            report.Activities.Add(new Activity() { Date = report.From, Amount = 1, PersonId = personId, StaffId = staffId, Type = ActivityType.Lv02 });
            report.Activities.Add(new Activity() { Date = report.From, Amount = 1, PersonId = personId, StaffId = staffId, Type = ActivityType.Lv04 });
            report.Activities.Add(new Activity() { Date = report.From, Amount = 1, PersonId = personId, StaffId = staffId, Type = ActivityType.Lv15 });
        }

        private void AddConsultation(HkpvReport report, string staffId)
        {
            report.Consultations.Add(new Consultation() { Date = report.From, Amount = 1, StaffId = staffId, Type = ConsultationType.Lv31 });
            report.Consultations.Add(new Consultation() { Date = report.From, Amount = 1, StaffId = staffId, Type = ConsultationType.Lv32 });
        }

        public HkpvReport Report { get; private set; }

        public HkpvReportValidationResult Result
        {
            get
            {
                if (_result == null)
                {
                    _result = (HkpvReportValidationResult)Report.Validate();
                }

                return _result;
            }
        }

        [Given(@"eine Meldung ist korrekt befüllt")]
        public void GivenAValidReport()
        {
            // nichts zu tun
        }

        [Given(@"die Eigenschaft '(\w*)' von '(\w*)' ist nicht gesetzt")]
        public void GivenThePropertyIsDefault(string name, string type)
        {
            if (type == nameof(HkpvReport))
                this.Report.SetDefault(name);            
            else if (type == nameof(Person))
                this.Report.Persons[0].SetDefault(name);
            else if (type == nameof(Staff))
                this.Report.Staffs[0].SetDefault(name);
            else if (type == nameof(Activity))
                foreach (var a in this.Report.Activities)
                    a.SetDefault(name);
            else if (type == nameof(Consultation))
                foreach (var c in this.Report.Consultations)
                    c.SetDefault(name);
            else
                throw new NotImplementedException();
        }

        [Given(@"die Eigenschaft '(\w*)' von '(\w*)' ist auf '(.*)' gesetzt")]
        public void GivenThePropertyIsSetTo(string name, string type, string value)
        {
            if (type == nameof(HkpvReport))
                this.Report.SetValue(name, value);            
            else if (type == nameof(Person))
                this.Report.Persons[0].SetValue(name, value);
            else if (type == nameof(Staff))
                this.Report.Staffs[0].SetValue(name, value);
            else if (type == nameof(Activity))
                foreach (var a in this.Report.Activities)
                    a.SetValue(name, value);
            else if (type == nameof(Consultation))
                foreach (var c in this.Report.Consultations)
                    c.SetValue(name, value);
            else
                throw new NotImplementedException();
        }

        [Given(@"die Meldung enthält am '(.*)' die Aktivitäten '(.*)'")]
        public void GivenTheActivitiesAt(string date, string values)
        {
            foreach (var lv in values.Split(',').Select(x => int.Parse(x)).GroupBy(x => x))
            {
                this.Report.Activities.Add(new Activity() { Amount = lv.Count(), Date = date, PersonId = this.Report.Persons[0].Id, StaffId = this.Report.Staffs[0].Id, Type = (ActivityType)lv.Key });
            }
        }


        [Given(@"die Meldung enthält die Aktivitäten '(.*?)'")]
        public void GivenTheActivities(string values)
        {
            foreach (var lv in values.Split(',').Select(x => int.Parse(x)).GroupBy(x => x))
            {
                this.Report.Activities.Add(new Activity() { Amount = lv.Count(), Date = this.Report.To, PersonId = this.Report.Persons[0].Id, StaffId = this.Report.Staffs[0].Id, Type = (ActivityType)lv.Key });
            }
        }

        [Given(@"die Meldung enthält bei der Person '(.*)' die Aktivitäten '(.*)'")]
        public void GivenTheActivitiesAtPerson(string personId, string values)
        {
            foreach (var lv in values.Split(',').Select(x => int.Parse(x)).GroupBy(x => x))
            {
                this.Report.Activities.Add(new Activity() { Amount = lv.Count(), Date = this.Report.To, PersonId = personId, StaffId = this.Report.Staffs[0].Id, Type = (ActivityType)lv.Key });
            }
        }


        [Given(@"die Meldung enthält von der Mitarbeiterin '(.*)' die Aktivitäten '(.*)'")]
        public void GivenTheActivitiesFromStaff(string staffId, string values)
        {
            foreach (var lv in values.Split(',').Select(x => int.Parse(x)).GroupBy(x => x))
            {
                this.Report.Activities.Add(new Activity() { Amount = lv.Count(), Date = this.Report.To, PersonId = this.Report.Persons[0].Id, StaffId = staffId, Type = (ActivityType)lv.Key });
            }
        }

        [Given(@"eine Versicherungsnummer ist nicht eindeutig")]
        public void GivenSsnNotUnique()
        {
            var p0 = this.Report.Persons[0];

            var p = this.Report.AddDummyPerson();

            p.Ssn = p0.Ssn;
        }

        [Given(@"der Id einer Person ist nicht eindeutig")]
        public void GivenPersonIdNotUnique()
        {
            var p0 = this.Report.Persons[0];

            var p = this.Report.AddDummyPerson();

            p.Id = p0.Id;
            p.Id = p0.Id;
        }

        [Given(@"der Id einer Mitarbeiterin ist nicht eindeutig")]
        public void GivenStaffIdNotUnique()
        {
            var s0 = this.Report.Staffs[0];

            var s = this.Report.AddDummyStaff();

            s.Id = s0.Id;
        }


        [Given(@"eine Auszubildende hat die Aktivitäten '(.*)' dokumentiert")]
        public void GivenTraineeWithActivity(string values)
        {
            var s = this.Report.AddDummyStaff();
            s.Role = StaffRole.Trainee;
            var staffId = s.Id;

            foreach (var lv in values.Split(',').Select(x => int.Parse(x)).GroupBy(x => x))
            {
                this.Report.Activities.Add(new Activity() { Amount = lv.Count(), Date = this.Report.To, PersonId = this.Report.Persons[0].Id, StaffId = staffId, Type = (ActivityType)lv.Key });
            }
        }

        [Given(@"zu einer Person sind keine Aktivitäten dokumentiert")]
        public void GivenPersonWithoutActivity()
        {
            this.Report.AddDummyPerson();
        }


        [Given(@"zu einer Mitarbeiterin sind keine Aktivitäten dokumentiert")]
        public void GivenStaffWithoutActivity()
        {
            this.Report.AddDummyPerson();
        }


        [Given(@"die Meldung enthält die Beratungen '(.*?)'")]
        public void GivenTheConsultations(string values)
        {
            foreach (var lv in values.Split(',').Select(x => int.Parse(x)).GroupBy(x => x))
            {
                this.Report.Consultations.Add(new Consultation() { Amount = lv.Count(), Date = this.Report.To, StaffId = this.Report.Staffs[0].Id, Type = (ConsultationType)lv.Key });
            }
        }


        [Given(@"die Meldung enthält am '(.*)' die Beratungen '(.*)'")]
        public void GivenTheConsultationsAt(string date, string values)
        {
            foreach (var lv in values.Split(',').Select(x => int.Parse(x)).GroupBy(x => x))
            {
                this.Report.Consultations.Add(new Consultation() { Amount = lv.Count(), Date = date, StaffId = this.Report.Staffs[0].Id, Type = (ConsultationType)lv.Key });
            }
        }


        [Then(@"*enthält (das Validierungsergebnis )?genau einen Fehler")]
        public void ThenTheResultContainsOneError(object test)
        {
            Assert.False(this.Result.IsValid);
            Assert.Single(this.Result.Errors.Where(x => x.Severity == Severity.Error).Select(x => x.ErrorMessage).Distinct());
        }

        [Then(@"*enthält (das Validierungsergebnis )?keine Fehler")]
        public void ThenTheResultContainsNoErrors(string dummy)
        {
            Assert.True(this.Result.IsValid);
            Assert.Empty(this.Result.Errors.Where(x => x.Severity == Severity.Error));
        }

        [Then(@"*enthält (das Validierungsergebnis )?keine Warnungen")]
        public void ThenTheResultContainsNoWarnings(string dummy)
        {
            Assert.Empty(this.Result.Errors.Where(x => x.Severity == Severity.Warning));
        }

        [Then(@"die Fehlermeldung lautet: '(.*)'")]
        public void ThenTheResultContainsJust(string message)
        {
            Assert.Equal(message, this.Result.Errors.Select(x => x.ErrorMessage).Distinct().Single());
        }

        [Then(@"enthält das Validierungsergebnis den Fehler '(.*)'")]
        public void ThenTheResultContainsAnError(string message)
        {
            var pattern = new Regex(message, RegexOptions.IgnoreCase);

            Assert.NotEmpty(this.Result.Errors.Where(x => x.Severity == Severity.Error && pattern.IsMatch(x.ErrorMessage)));
        }

        [Then(@"enthält das Validierungsergebnis die Warnung '(.*)'")]
        public void ThenTheResultContainsAnWarning(string message)
        {
            var pattern = new Regex(message, RegexOptions.IgnoreCase);

            Assert.NotEmpty(this.Result.Errors.Where(x => x.Severity == Severity.Warning && pattern.IsMatch(x.ErrorMessage)));
        }
    }
}
