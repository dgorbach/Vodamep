using System;

namespace Vodamep.Data
{
    public sealed class QualificationCodeProvider : CodeProviderBase
    {
        private static volatile QualificationCodeProvider instance;
        private static readonly object syncRoot = new Object();        

        public static QualificationCodeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new QualificationCodeProvider();
                    }
                }

                return instance;
            }
        }

        protected override string ResourceName => "Data.qualifications.csv";
        public override string Unknown => "";
        public string Trainee => "AB";
    }
}
