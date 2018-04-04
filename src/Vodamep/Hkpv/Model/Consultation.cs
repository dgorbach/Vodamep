using System;

namespace Vodamep.Hkpv.Model
{
    public partial class Consultation : IComparable<Consultation>
    {
        public int CompareTo(Consultation other)
        {
            int result;

            if ((result = this.Date.CompareTo(other.Date)) != 0)
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
                case ConsultationType.Lv32:
                case ConsultationType.Lv34:
                    return 0;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}