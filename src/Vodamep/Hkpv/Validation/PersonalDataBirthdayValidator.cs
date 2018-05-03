using FluentValidation;
using System;
using Vodamep.Hkpv.Model;


namespace Vodamep.Hkpv.Validation
{

    internal class PersonalDataBirthdayValidator : AbstractValidator<PersonalData>
    {
        public PersonalDataBirthdayValidator()
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Birthday)
                .NotEmpty();

            RuleFor(x => x.Birthday)
                .SetValidator(new DateTimeValueValidator());

            RuleFor(x => x.BirthdayD)
                .LessThan(DateTime.Today)
                .Unless(x => string.IsNullOrEmpty(x.Birthday))
                .WithMessage(Validationmessages.BirthdayNotInFuture);

            RuleFor(x => x.BirthdayD)
               .GreaterThanOrEqualTo(new DateTime(1900, 01, 01))
               .Unless(x => string.IsNullOrEmpty(x.Birthday));

            RuleFor(x => x.BirthdayD)
                .Must(CheckDates)
                .Unless(x => string.IsNullOrEmpty(x.Birthday) || !SSNHelper.IsValid(x.Ssn))
                .WithSeverity(Severity.Warning)                
                .WithMessage(x => Validationmessages.BirthdayNotInSsn(x));  
        }


        private bool CheckDates(PersonalData data, DateTime date)
        {
            if (date == null) return false;

            var date2 = SSNHelper.GetBirthDay(data.Ssn);

            if (!date2.HasValue) return true;

            return date2.Value.ToString("ddMMyy") == date.ToString("ddMMyy");
        }
    }
}
