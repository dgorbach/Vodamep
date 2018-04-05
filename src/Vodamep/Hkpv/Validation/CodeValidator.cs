using FluentValidation.Validators;
using Vodamep.Data;

namespace Vodamep.Hkpv.Validation
{
    public class CodeValidator<T> : PropertyValidator
        where T: CodeProviderBase
    {
        public CodeValidator()
            : base(Validationmessages.InvalidCode)
        {

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var code = context.PropertyValue as string;

            if (string.IsNullOrEmpty(code)) return true;

            var provider = CodeProviderBase.GetInstance<T>();

            return provider.IsValid(code);
        }
    }
}
