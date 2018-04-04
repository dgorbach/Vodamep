using FluentValidation;
using Vodamep.Hkpv.Model;
using Vodamep.Model;

namespace Vodamep.Hkpv.Validation
{
    public class ConsultationValidator : AbstractValidator<Consultation>
    {
        public ConsultationValidator(LocalDate from, LocalDate to)
        {
            this.RuleFor(x => x.Date).NotNull();
            this.RuleFor(x => x.Date).SetValidator(new LocalDateValidator());
            if (from != null)
            {
                this.RuleFor(x => x.Date).GreaterThanOrEqualTo(from);
            }

            if (to != null)
            {
                this.RuleFor(x => x.Date).LessThanOrEqualTo(to);
            }

            this.RuleFor(x => x.StaffId).NotNull();
            this.RuleFor(x => x.Amount).GreaterThan(0);
            this.RuleFor(x => x.Type).NotEqual(ConsultationType.UnknownConsultation);


        }
    }

}
