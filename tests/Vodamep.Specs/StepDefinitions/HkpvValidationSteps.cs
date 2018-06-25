using FluentValidation;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
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
        private Activity _dummyActivities;

        public HkpvValidationSteps()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

            var loc = new DisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            var date = DateTime.Today.AddMonths(-1);
            this.Report = DataGenerator.Instance.CreateHkpvReport(date.Year, date.Month, 1, 1, false);

            this.AddDummyActivities(Report.Persons[0].Id, Report.Staffs[0].Id);

            var consultation = new Activity() { Date = this.Report.From, StaffId = Report.Staffs[0].Id };
            consultation.Entries.Add(ActivityType.Lv31);
            this.Report.Activities.Add(consultation);
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

            else
                throw new NotImplementedException();
        }

        [Given(@"die Meldung enthält am '(.*)' die Aktivitäten '(.*)'")]
        public void GivenTheActivitiesAt(string date, string values)
        {
            var d = Timestamp.FromDateTime(date.AsDate());

            this.GivenTheActivitiesAt(this.Report.Persons[0].Id, d, values, this.Report.Staffs[0].Id);
        }


        [Given(@"die Meldung enthält die Aktivitäten '(.*?)'")]
        public void GivenTheActivities(string values)
        {
            this.GivenTheActivitiesAt(this.Report.Persons[0].Id, this.Report.To, values, this.Report.Staffs[0].Id);
        }

        [Given(@"die Meldung enthält jeden Tag die Aktivitäten '(.*?)'")]
        public void GivenTheDailyActivities(string values)
        {
            this.RemoveDummyActivities();

            foreach (var day in Enumerable.Range(1, this.Report.ToD.Day))
            {
                this.GivenTheActivitiesAt(this.Report.Persons[0].Id, new DateTime(this.Report.ToD.Year, this.Report.ToD.Month, day).AsTimestamp(), values, this.Report.Staffs[0].Id);
            }
        }

        [Given(@"die Meldung enthält bei der Person '(.*)' die Aktivitäten '(.*)'")]
        public void GivenTheActivitiesAtPerson(string personId, string values)
        {
            this.GivenTheActivitiesAt(personId, this.Report.To, values, this.Report.Staffs[0].Id);
        }


        [Given(@"die Meldung enthält von der Mitarbeiterin '(.*)' die Aktivitäten '(.*)'")]
        public void GivenTheActivitiesFromStaff(string staffId, string values)
        {
            this.GivenTheActivitiesAt(this.Report.Persons[0].Id, this.Report.To, values, staffId);
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

            this.GivenTheActivitiesAt(this.Report.Persons[0].Id, this.Report.To, values, staffId);
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
        [Given(@"die Meldung enthält am '(.*)' die Beratungen '(.*)'")]
        public void GivenTheConsultationsAt(string date, string values)
        {
            var d = Timestamp.FromDateTime(date.AsDate());

            this.GivenTheActivitiesAt(string.Empty, d, values, this.Report.Staffs[0].Id);
        }


        [Given(@"die Datums-Eigenschaft '(\w*)' von '(\w*)' hat eine Uhrzeit gesetzt")]
        public void GivenThePropertyHasATime(string name, string type)
        {
            IMessage m;
            if (type == nameof(HkpvReport))
                m = this.Report;
            else if (type == nameof(Person))
                m = this.Report.Persons[0];
            else if (type == nameof(Staff))
                m = this.Report.Staffs[0];
            else if (type == nameof(Activity))
                m = this.Report.Activities[0];
            else
                throw new NotImplementedException();

            var field = m.GetField(name);
            var ts = (field.Accessor.GetValue(m) as Timestamp) ?? this.Report.From;

            ts.Seconds = ts.Seconds + 60 * 60;
            field.Accessor.SetValue(m, ts);
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


        private void GivenTheActivitiesAt(string personId, Timestamp d, string values, string staffId)
        {
            this.RemoveDummyActivities();

            var a = new Activity() { Date = d, PersonId = personId, StaffId = staffId };
            a.Entries.AddRange(values.Split(',').Select(x => (ActivityType)int.Parse(x)));

            this.Report.Activities.Add(a);
        }

        private void AddDummyActivities(string personId, string staffId)
        {

            _dummyActivities = new Activity() { Date = this.Report.From, PersonId = personId, StaffId = staffId };
            _dummyActivities.Entries.Add(new[] { ActivityType.Lv02, ActivityType.Lv04, ActivityType.Lv15 });

            this.Report.Activities.Add(_dummyActivities);
        }

        private void RemoveDummyActivities()
        {
            if (_dummyActivities != null)
            {
                this.Report.Activities.Remove(_dummyActivities);
                _dummyActivities = null;
            }
        }
    }
}
