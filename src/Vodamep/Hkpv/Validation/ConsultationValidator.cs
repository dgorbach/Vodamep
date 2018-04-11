using FluentValidation;
using System;
using Vodamep.Hkpv.Model;


namespace Vodamep.Hkpv.Validation
{
    public class ConsultationValidator : AbstractValidator<Consultation>
    {
        public ConsultationValidator(DateTime from, DateTime to)
        {
            this.RuleFor(x => x.Date).NotEmpty();
            this.RuleFor(x => x.Date).SetValidator(new DateTimeValueValidator());

            if (from != DateTime.MinValue)
            {
                this.RuleFor(x => x.DateD).GreaterThanOrEqualTo(from).Unless(x => string.IsNullOrEmpty(x.Date));
            }
            if (to > from)
            {
                this.RuleFor(x => x.DateD).LessThanOrEqualTo(to).Unless(x => string.IsNullOrEmpty(x.Date));
            }

            this.RuleFor(x => x.StaffId).NotNull();            
            this.RuleFor(x => x.Amount).GreaterThan(0);
            this.RuleFor(x => x.Type).NotEqual(ConsultationType.UndefinedConsultation);
        }
    }

}
