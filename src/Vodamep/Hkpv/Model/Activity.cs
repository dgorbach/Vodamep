using System;

namespace Vodamep.Hkpv.Model
{
    public partial class Activity : IComparable<Activity>
    {

        public DateTime DateD { get => this.Date.AsDate(); set => this.Date = value.AsValue(); }

        public int CompareTo(Activity other)
        {
            int result;

            if ((result = this.DateD.CompareTo(other.DateD)) != 0)
                return result;

            if ((result = this.PersonId.CompareTo(other.PersonId)) != 0)
                return result;

            if ((result = this.StaffId.CompareTo(other.StaffId)) != 0)
                return result;

            if ((result = this.Type.CompareTo(other.Type)) != 0)
                return result;

            return result;
        }

        public int GetMinutes() => GetLP() * 5;
       
        public int GetLP()
        {
            switch (this.Type)
            {
                case ActivityType.Lv01:
                case ActivityType.Lv06:
                case ActivityType.Lv08:
                case ActivityType.Lv15:
                    return 1;
                case ActivityType.Lv02:
                case ActivityType.Lv05:
                case ActivityType.Lv07:
                case ActivityType.Lv09:
                case ActivityType.Lv10:
                case ActivityType.Lv11:
                case ActivityType.Lv12:
                case ActivityType.Lv13:
                case ActivityType.Lv16:
                    return 2;
                case ActivityType.Lv14:
                case ActivityType.Lv17:
                    return 3;
                case ActivityType.Lv03:
                case ActivityType.Lv04:
                    return 4;
                case ActivityType.Lv31:
                case ActivityType.Lv33:
                    return 1;
                default:
                    throw new NotImplementedException();
            }
        }


        public static ActivityType[] ActivityTypesWithoutPerson = new[] { ActivityType.Lv31, ActivityType.Lv33 };
    }
}