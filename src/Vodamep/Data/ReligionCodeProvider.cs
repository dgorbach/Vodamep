using System;

namespace Vodamep.Data
{
    public sealed class ReligionCodeProvider : CodeProviderBase
    {
        private static volatile ReligionCodeProvider instance;
        private static object syncRoot = new Object();

        public static ReligionCodeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ReligionCodeProvider();
                    }
                }

                return instance;
            }
        }

        protected override string ResourceName => "Data.religions.csv";
    }
}
