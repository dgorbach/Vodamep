using FluentValidation;
using System;
using Vodamep.Hkpv.Model;
using Vodamep.Model;

namespace Vodamep.Hkpv.Validation
{
    public class HkpvReportValidator : AbstractValidator<HkpvReport>
    {
        public HkpvReportValidator()
        {
            this.RuleFor(x => x.Institution).NotNull();
            this.RuleFor(x => x.Institution).SetValidator(new InstitutionValidator());
            this.RuleFor(x => x.From).NotNull();
            this.RuleFor(x => x.To).NotNull();
            this.RuleFor(x => x.To).LessThanOrEqualTo(x => LocalDate.Today);
            this.RuleFor(x => x.To).GreaterThan(x => x.From).Unless(x => x.From == null || x.To == null);

            //corert kann derzeit nicht mit AnonymousType umgehen. Vielleicht später: this.RuleFor(x => new { x.From, x.To })
            this.RuleFor(x => new Tuple<LocalDate, LocalDate>(x.From, x.To))
                .Must(x => x.Item2 == x.Item1.LastDateInMonth())
                .Unless(x => x.From == null || x.To == null)
                .WithMessage(Validationmessages.OneMonth);

            this.RuleFor(x => x.To)
                .Must(x => x == x.LastDateInMonth())
                .Unless(x => x.To == null)
                .WithMessage(Validationmessages.LastDateInMonth);

            this.RuleFor(x => x.From)
                .Must(x => x.Day == 1)
                .Unless(x => x.From == null)
                .WithMessage(Validationmessages.FirstDateInMOnth);

            this.RuleFor(x => x.From).SetValidator(new LocalDateValidator());
            this.RuleFor(x => x.To).SetValidator(new LocalDateValidator());

            this.RuleForEach(report => report.PersonalData).SetValidator(new PersonalDataValidator());

            this.RuleForEach(report => report.Persons).SetValidator(new PersonValidator());

            this.RuleForEach(report => report.Activities).SetValidator(r => new ActivityValidator(r.From, r.To));

            this.RuleForEach(report => report.OtherActivities).SetValidator(r => new ActivityValidator(r.From, r.To));

            this.RuleForEach(report => report.Consultations).SetValidator(r => new ConsultationValidator(r.From, r.To));

            this.RuleForEach(report => report.Staffs).SetValidator(new StaffValidator());

            this.Include(new ActivityValidator123And15());

            this.Include(new ActivityIsUniqueValidator());

            this.Include(new ActivityMedicalByStaffRoleTraineeValidator());

            this.Include(new HkpvReportPersonIdValidator());

            this.Include(new HkpvReportStaffIdValidator());

            this.Include(new PersonalDataSsnIsUniqueValidator());

            this.Include(new ConsultationsValidator3132And3334());
        }
    }
}
