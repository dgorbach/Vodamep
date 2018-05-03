using FluentValidation.Validators;
using System;
using System.Globalization;

namespace Vodamep.Hkpv.Validation
{
    internal class DateTimeValueValidator : PropertyValidator
    {
        public DateTimeValueValidator()
            : base(Validationmessages.LocalDate)
        {

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var date = context.PropertyValue as string;

            return string.IsNullOrEmpty(date) || IsValid(date);
        }

        public static bool IsValid(string value) => DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
    }
}
