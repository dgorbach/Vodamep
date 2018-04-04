namespace Vodamep.Hkpv.Model
{
    public partial class Person
    {
        public static (Person person, PersonalData data) Create(string id, string religion, string name)
        {
            return (Person.Create(id, religion), new PersonalData() { Id = id, FamilyName = name });
        }
        public static Person Create(string id, string religion)
        {
            var result = new Person()
            {
                Id = id,
                Religion = religion
            };

            return result;
        }

    }
}