using System;

namespace Vodamep.Hkpv.Model
{
    public static class ActivityTypeExtensions
    {

        public static int GetMinutes(this ActivityType type) => type.GetLP() * 5;

        public static int GetLP(this ActivityType type)
        {
            switch (type)
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

        public static bool IsMedical(this ActivityType type) => type == ActivityType.Lv06 || type == ActivityType.Lv07 || type == ActivityType.Lv08 || type == ActivityType.Lv09 || type == ActivityType.Lv10;

        public static bool RequiresPersonId(this ActivityType type) => !type.WithoutPersonId();

        public static bool WithoutPersonId(this ActivityType type) => type == ActivityType.Lv31 || type == ActivityType.Lv33;

        
    }

}