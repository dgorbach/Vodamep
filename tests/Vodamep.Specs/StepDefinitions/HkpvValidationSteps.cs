using FluentValidation;
using System;
using System.Globalization;
using System.Linq;
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
            this.Report = DataGenerator.Instance.CreateHkpvReport(date.Year, date.Month, 1, 1, true);
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
        public void GivenThePropertyOfPersonalDataIsDefault(string name, string type)
        {
            if (type == nameof(HkpvReport))
                this.Report.SetDefault(name);
            else if (type == nameof(PersonalData))
                this.Report.PersonalData[0].SetDefault(name);
            else if (type == nameof(Person))
                this.Report.Persons[0].SetDefault(name);
            else if (type == nameof(Staff))
                this.Report.Staffs[0].SetDefault(name);
            

            else
                throw new NotImplementedException();
        }

        [Then(@"*enthält (das Validierungsergebnis )?genau einen Fehler")]
        public void ThenTheResultContainsOneError(object test)
        {
            Assert.False(this.Result.IsValid);
            Assert.Single(this.Result.Errors.Where(x => x.Severity == Severity.Error));
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
        public void Then(string message)
        {
            Assert.Equal(message, this.Result.Errors.Single().ErrorMessage);
        }

    }
}
