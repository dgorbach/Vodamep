using System;

namespace Vodamep.Hkpv.Model
{
    public partial class Person
    {
        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsValue(); }

    }
}