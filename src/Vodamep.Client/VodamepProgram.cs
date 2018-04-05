using PowerArgs;
using System;
using System.IO;
using Vodamep.Data;
using Vodamep.Data.Dummy;

namespace Vodamep.Client
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class VodamepProgram
    {
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("schreibt -'Das ist ein Test'"),]
        public void Test()
        {
            Console.WriteLine("Das ist ein Test");
        }

        [ArgActionMethod]
        public void Validate(ValidateArgs args)
        {
            Console.WriteLine($"validate {args.File}");
        }


        [ArgActionMethod, ArgDescription("Pack some random data.")]
        public void PackRandom(PackRandomArgs args)
        {

            int? year = args.Year;
            if (year < 2000 || year > DateTime.Today.Year) year = null;

            int? month = args.Month;
            if (month < 1 || month > 12) month = null;

            var r = DataGenerator.Instance.CreateHkpvReport(year, month, args.Persons, args.Staffs, args.AddActivities);

            if (args.Json)
            {
                using (var s = File.OpenWrite("test.json"))
                using (var ss = new StreamWriter(s))
                {
                    Google.Protobuf.JsonFormatter.Default.WriteValue(ss, r);
                }

            }
            else
            {
                using (var s = File.OpenWrite("test.hkpv"))
                {
                    Google.Protobuf.MessageExtensions.WriteTo(r, s);
                }
            }

            Console.WriteLine($"packrandom ");
        }

        [ArgActionMethod]
        public void List(ListArgs args)
        {
            CodeProviderBase provider = null;

            switch (args.Source)
            {
                case ListSources.Religions:
                    provider = ReligionCodeProvider.Instance;
                    break;
                case ListSources.Insurances:
                    provider = InsuranceCodeProvider.Instance;
                    break;
                case ListSources.CountryCodes:
                    provider = CountryCodeProvider.Instance;
                    break;
            }

            foreach (var line in provider?.GetCSV())
                Console.WriteLine(line);

        }
    }

}
