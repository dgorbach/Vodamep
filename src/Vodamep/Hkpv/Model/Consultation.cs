using System;

namespace Vodamep.Hkpv.Model
{
    public partial class Consultation : IComparable<Consultation>
    {
        public DateTime DateD { get => this.Date.AsDate(); set => this.Date = value.AsValue(); }

        public int CompareTo(Consultation other)
        {
            int result;

            if ((result = this.DateD.CompareTo(other.DateD)) != 0)
                return result;

            if ((result = this.StaffId.CompareTo(other.StaffId)) != 0)
                return result;

            if ((result = this.Type.CompareTo(other.Type)) != 0)
                return result;

            return result;
        }

        public int GetMinutes()
        {
            switch (this.Type)
            {
                case ConsultationType.Lv31:
                case ConsultationType.Lv33:
                    return 5;               
                default:
                    throw new NotImplementedException();
            }
        }
    }
}