using FluentValidation;
using System.Text.RegularExpressions;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class StaffValidator : AbstractValidator<Staff>
    {
        public StaffValidator()
        {
            this.RuleFor(x => x.FamilyName).NotEmpty();
            this.RuleFor(x => x.GivenName).NotEmpty();

            // Wunsch von Gerhard
            // aus unterlagen_connexia: Daten.xsl
            var r = new Regex("^[a-zA-ZäöüÄÖÜß][-,.a-zA-ZäöüÄÖÜß ]*[,.a-zA-ZäöüÄÖÜß]$");
            this.RuleFor(x => x.FamilyName).Matches(r).Unless(x => string.IsNullOrEmpty(x.FamilyName));
            this.RuleFor(x => x.GivenName).Matches(r).Unless(x => string.IsNullOrEmpty(x.GivenName));
        }
    }
}
