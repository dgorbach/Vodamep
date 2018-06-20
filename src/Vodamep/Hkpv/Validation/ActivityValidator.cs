using FluentValidation;
using System;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator(DateTime from, DateTime to)
        {
            this.RuleFor(x => x.Date).NotEmpty();
            this.RuleFor(x => x.Date).SetValidator(new DateTimeValueValidator()).Unless(x => x.Date == null);

            if (from != DateTime.MinValue)
            {
                this.RuleFor(x => x.DateD).GreaterThanOrEqualTo(from).Unless(x => x.Date == null);
            }
            if (to > from)
            {
                this.RuleFor(x => x.DateD).LessThanOrEqualTo(to).Unless(x => x.Date == null);
            }

            this.RuleFor(x => x.StaffId).NotEmpty();

            this.RuleFor(x => x.PersonId).NotEmpty().Unless(x => Activity.ActivityTypesWithoutPerson.Contains(x.Type));
            this.RuleFor(x => x.PersonId).Empty().Unless(x => !Activity.ActivityTypesWithoutPerson.Contains(x.Type));

            this.RuleFor(x => x.Amount).GreaterThan(0);
            this.RuleFor(x => x.Type).NotEqual(ActivityType.UndefinedActivity);
        }
    }

}
