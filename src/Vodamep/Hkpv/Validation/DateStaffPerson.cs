using System;

namespace Vodamep.Hkpv.Validation
{
    public class DateStaffPerson : IEquatable<DateStaffPerson>
    {
        public DateTime Date { get; set; }
        public string StaffId { get; set; }
        public string PersonId { get; set; }

        public bool Equals(DateStaffPerson other)
        {
            if (!(other is null))
                return other.Date == this.Date && other.StaffId == this.StaffId && other.PersonId == this.PersonId;

            return false;
        }

        public override int GetHashCode()
        {
            int r = 0;

            if (this.Date != null)
                r = r ^ this.Date.GetHashCode();

            if (this.PersonId != null)
                r = r ^ this.PersonId.GetHashCode();

            if (this.StaffId != null)
                r = r ^ this.StaffId.GetHashCode();

            return r;
        }
    }
}
