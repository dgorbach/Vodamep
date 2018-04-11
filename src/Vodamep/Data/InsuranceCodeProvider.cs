using System;

namespace Vodamep.Data
{
    public sealed class InsuranceCodeProvider : CodeProviderBase
    {
        private static volatile InsuranceCodeProvider instance;
        private static object syncRoot = new Object();

        public static InsuranceCodeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new InsuranceCodeProvider();
                    }
                }

                return instance;
            }
        }
        
        protected override string ResourceName => "Data.insurances.csv";

        public override string Unknown => "UB";
    }
}
