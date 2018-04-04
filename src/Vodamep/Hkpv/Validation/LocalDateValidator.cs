using FluentValidation.Validators;
using Vodamep.Model;

namespace Vodamep.Hkpv.Validation
{
    public class LocalDateValidator : PropertyValidator
    {
        public LocalDateValidator()
            : base(Validationmessages.LocalDate)
        {

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var date = context.PropertyValue as LocalDate;

            return date == null || date.IsValid();
        }
    }
}
