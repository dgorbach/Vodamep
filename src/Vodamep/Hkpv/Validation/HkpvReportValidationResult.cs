using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace Vodamep.Hkpv.Validation
{
    public class HkpvReportValidationResult : ValidationResult
    {
        public HkpvReportValidationResult(ValidationResult result)
            : base(result.Errors)
        {

        }
        public override bool IsValid => this.Errors.Where(x => x.Severity == Severity.Error).Count() == 0;
    }
}
