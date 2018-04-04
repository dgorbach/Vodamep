using FluentValidation;
using Vodamep.Hkpv.Model;
using Vodamep.Model;

namespace Vodamep.Hkpv.Validation
{

    public class PersonalDataBirthdayValidator : AbstractValidator<PersonalData>
    {
        public PersonalDataBirthdayValidator()
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Birthday)
                .NotNull();

            RuleFor(x => x.Birthday)
                .SetValidator(new LocalDateValidator());

            RuleFor(x => x.Birthday)
                .LessThan(LocalDate.Today)
                .WithMessage(Validationmessages.BirthdayNotInFuture);

            RuleFor(x => x.Birthday)
               .GreaterThanOrEqualTo(new LocalDate(1900, 01, 01));

            RuleFor(x => x.Birthday)
                .Must(CheckDates)
                .Unless(x => x.Birthday == null || !SSNHelper.IsValid(x.Ssn))
                .WithMessage(x => Validationmessages.BirthdayNotInSsn(x));
        }


        private bool CheckDates(PersonalData data, LocalDate date)
        {
            if (date == null) return false;

            var date2 = SSNHelper.GetBirthDay(data.Ssn);

            if (!date2.HasValue) return true;

            return date2.Value.ToString("ddMMyy") == date.ToString("ddMMyy");
        }
    }
}
