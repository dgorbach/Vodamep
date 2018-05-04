using FluentValidation;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class PersonSsnValidator : AbstractValidator<Person>
    {
        public PersonSsnValidator()   
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Ssn)
                .NotEmpty();

            RuleFor(x => x.Ssn)                
                .Must(x => SSNHelper.IsValid(x))
                .WithMessage(Validationmessages.SsnNotValid);            
        }

    }
}
