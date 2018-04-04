using FluentValidation;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            this.RuleFor(client => client.Insurance).NotEmpty();
            this.RuleFor(client => client.Religion).NotEmpty();
        }
    }
}
