using FluentValidation.Validators;
using Google.Protobuf.WellKnownTypes;

namespace Vodamep.Hkpv.Validation
{
    internal class TimestampWithOutTimeValidator : PropertyValidator
    {
        public TimestampWithOutTimeValidator()
            : base(Validationmessages.DateMustnotHaveTime)
        {

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (!(context.PropertyValue is Timestamp ts)) return true;

            var date = ts.ToDateTime();

            return date.Date == date; 
        }
    }
}
