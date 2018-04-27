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

            this.Report = DataGenerator.Instance.CreateHkpvReport(null, null, 0, 0, false);
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

        [Given(@"die Eigenschaft '(.*)' ist nicht gesetzt")]
        public void GivenThePropertyIsDefault(string name)
        {
            var field = HkpvReport.Descriptor.Fields.InDeclarationOrder().Where(x => string.Equals(x.Name, name, System.StringComparison.CurrentCultureIgnoreCase)).First();

            switch (field.FieldType)
            {
                case Google.Protobuf.Reflection.FieldType.String:
                    field.Accessor.SetValue(this.Report, string.Empty);
                    break;
                case Google.Protobuf.Reflection.FieldType.Int64:
                case Google.Protobuf.Reflection.FieldType.Int32:
                case Google.Protobuf.Reflection.FieldType.SInt32:
                case Google.Protobuf.Reflection.FieldType.SInt64:
                case Google.Protobuf.Reflection.FieldType.UInt32:
                case Google.Protobuf.Reflection.FieldType.UInt64:
                case Google.Protobuf.Reflection.FieldType.SFixed32:
                case Google.Protobuf.Reflection.FieldType.SFixed64:
                case Google.Protobuf.Reflection.FieldType.Double:
                case Google.Protobuf.Reflection.FieldType.Float:
                case Google.Protobuf.Reflection.FieldType.Enum:
                    field.Accessor.SetValue(this.Report, 0);
                    break;

                case Google.Protobuf.Reflection.FieldType.Message:
                    field.Accessor.SetValue(this.Report, null);
                    break;
                default:
                    throw new NotImplementedException();
            }            
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
