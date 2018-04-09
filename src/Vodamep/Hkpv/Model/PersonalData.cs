using System;

namespace Vodamep.Hkpv.Model
{
    public partial class PersonalData
    {
        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsValue(); }
    }
}