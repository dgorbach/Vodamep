using System;
using Vodamep.Data.Dummy;
namespace Vodamep.Hkpv.Model
{
    public partial class HkpvReport
    {
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsValue(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsValue(); }


        public static HkpvReport CreateDummyData()
        {
            var r = new HkpvReport()
            {
                Institution = new Institution()
                {
                    Id = "test",
                    Name = "Test"
                }
            };
            
            r.FromD = DateTime.Today.FirstDateInMonth().AddMonths(-1);
            r.ToD = r.FromD.LastDateInMonth();

            r.AddDummyPerson();
            r.AddDummyStaff();
            r.AddDummyActivities();

            return r;
            
        }

    }
}