using System;

namespace Vodamep.Data
{
    public sealed class Postcode_CityProvider : CodeProviderBase
    {
        private static volatile Postcode_CityProvider instance;
        private static object syncRoot = new Object();        

        public static Postcode_CityProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Postcode_CityProvider();
                    }
                }

                return instance;
            }
        }

        protected override string ResourceName => "Data.Postcode_Cities.txt";

        public override string Unknown => "";
    }
}
