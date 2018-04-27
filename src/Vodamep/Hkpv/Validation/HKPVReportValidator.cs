using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{

    public class HkpvReportValidator : AbstractValidator<HkpvReport>
    {
        static HkpvReportValidator()
        {
            var isGerman = Thread.CurrentThread.CurrentCulture.Name.StartsWith("de", StringComparison.CurrentCultureIgnoreCase);
            if (isGerman)
            {
                var loc = new DisplayNameResolver();
                ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
            }
        }
        public HkpvReportValidator()
        {
            this.RuleFor(x => x.Institution).NotEmpty();
            this.RuleFor(x => x.Institution).SetValidator(new InstitutionValidator());
            this.RuleFor(x => x.From).NotEmpty();
            this.RuleFor(x => x.To).NotEmpty();
            this.RuleFor(x => x.From).SetValidator(new DateTimeValueValidator());
            this.RuleFor(x => x.To).SetValidator(new DateTimeValueValidator());
            this.RuleFor(x => x.ToD).LessThanOrEqualTo(x => DateTime.Today);
            this.RuleFor(x => x.ToD).GreaterThan(x => x.FromD).Unless(x => string.IsNullOrEmpty(x.From) || string.IsNullOrEmpty(x.To));

            //corert kann derzeit nicht mit AnonymousType umgehen. Vielleicht später: this.RuleFor(x => new { x.From, x.To })
            this.RuleFor(x => new Tuple<DateTime, DateTime>(x.FromD, x.ToD))
                .Must(x => x.Item2 == x.Item1.LastDateInMonth())
                .Unless(x => string.IsNullOrEmpty(x.From) || string.IsNullOrEmpty(x.To))
                .WithMessage(Validationmessages.OneMonth);

            this.RuleFor(x => x.ToD)
                .Must(x => x == x.LastDateInMonth())
                .Unless(x => string.IsNullOrEmpty(x.To))
                .WithMessage(Validationmessages.LastDateInMonth);

            this.RuleFor(x => x.FromD)
                .Must(x => x.Day == 1)
                .Unless(x => string.IsNullOrEmpty(x.From))
                .WithMessage(Validationmessages.FirstDateInMOnth);



            this.RuleForEach(report => report.PersonalData).SetValidator(new PersonalDataValidator());

            this.RuleForEach(report => report.Persons).SetValidator(new PersonValidator());

            this.RuleForEach(report => report.Activities).SetValidator(r => new ActivityValidator(r.FromD, r.ToD));

            this.RuleForEach(report => report.Consultations).SetValidator(r => new ConsultationValidator(r.FromD, r.ToD));

            this.RuleForEach(report => report.Staffs).SetValidator(new StaffValidator());

            this.Include(new ActivityValidator123And15());

            this.Include(new ActivityIsUniqueValidator());

            this.Include(new ActivityMedicalByStaffRoleTraineeValidator());

            this.Include(new HkpvReportPersonIdValidator());

            this.Include(new HkpvReportStaffIdValidator());

            this.Include(new PersonalDataSsnIsUniqueValidator());

            this.Include(new ConsultationsValidator3132And3334());
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<HkpvReport> context, CancellationToken cancellation = default(CancellationToken))
        {
            return new HkpvReportValidationResult(await base.ValidateAsync(context, cancellation));
        }

        public override ValidationResult Validate(ValidationContext<HkpvReport> context)
        {
            return new HkpvReportValidationResult(base.Validate(context));
        }
    }
}
