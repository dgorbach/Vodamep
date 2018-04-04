using System;

namespace Vodamep.Hkpv.Model
{
    public partial class Activity : IComparable<Activity>
    {
        public int CompareTo(Activity other)
        {
            int result;

            if ((result = this.Date.CompareTo(other.Date)) != 0)
                return result;

            if ((result = this.PersonId.CompareTo(other.PersonId)) != 0)
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
                case ActivityType.Lv01:
                case ActivityType.Lv06:
                case ActivityType.Lv08:
                case ActivityType.Lv15:
                    return 5;
                case ActivityType.Lv02:
                case ActivityType.Lv05:
                case ActivityType.Lv07:
                case ActivityType.Lv09:
                case ActivityType.Lv10:
                case ActivityType.Lv11:
                case ActivityType.Lv12:
                case ActivityType.Lv13:
                case ActivityType.Lv16:
                    return 10;
                case ActivityType.Lv14:
                case ActivityType.Lv17:
                    return 15;
                case ActivityType.Lv03:
                case ActivityType.Lv04:
                    return 20;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}