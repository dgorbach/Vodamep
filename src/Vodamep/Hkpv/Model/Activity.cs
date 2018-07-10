using System;
using System.Linq;

namespace Vodamep.Hkpv.Model
{
    public partial class Activity : IComparable<Activity>
    {
        public DateTime DateD { get => this.Date.AsDate(); set => this.Date = value.AsTimestamp(); }

        public int CompareTo(Activity other)
        {
            int result;

            if ((result = this.DateD.CompareTo(other.DateD)) != 0)
                return result;

            if ((result = this.PersonId.CompareTo(other.PersonId)) != 0)
                return result;

            if ((result = this.StaffId.CompareTo(other.StaffId)) != 0)
                return result;

            for (var i = 0; i < this.Entries.Count; i++)
            {
                if (other.Entries.Count <= i)
                    return 1;

                if ((result = this.Entries[i].CompareTo(other.Entries[i])) != 0)
                    return result;
            }

            if (this.Entries.Count < other.Entries.Count)
                return -1;

            return 0;
        }

        public int GetMinutes() => GetLP() * 5;

        public int GetLP() => this.Entries.Sum(x => x.GetLP());
    }
}