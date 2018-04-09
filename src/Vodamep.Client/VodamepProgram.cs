using PowerArgs;
using System;
using System.IO;
using System.Linq;
using Vodamep.Data;
using Vodamep.Data.Dummy;
using Vodamep.Hkpv.Model;

namespace Vodamep.Client
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class VodamepProgram
    {
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod]
        public void Validate(ValidateArgs args)
        {
            Console.WriteLine($"validate {args.File}");
        }


        [ArgActionMethod, ArgDescription("Pack a file.")]
        public void PackFile(PackFileArgs args)
        {
            
            var content = File.ReadAllBytes(args.File);
            
            var isJson = System.Text.Encoding.UTF8.GetString(content.Take(5).ToArray()).Contains("{");
            
            HkpvReport r;

            if (isJson)
            {
                r = HkpvReport.Parser.ParseJson(System.Text.Encoding.UTF8.GetString(content));

            }
            else
            {
                r = HkpvReport.Parser.ParseFrom(content);
            }

            r = r.AsSorted();

            Write(r, args.Json);

        }

        [ArgActionMethod, ArgDescription("Pack some random data.")]
        public void PackRandom(PackRandomArgs args)
        {

            int? year = args.Year;
            if (year < 2000 || year > DateTime.Today.Year) year = null;

            int? month = args.Month;
            if (month < 1 || month > 12) month = null;

            var r = DataGenerator.Instance.CreateHkpvReport(year, month, args.Persons, args.Staffs, args.AddActivities).AsSorted();

            Write(r, args.Json);
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

        private void Write(HkpvReport r, bool asJson)
        {
            if (asJson)
            {
                using (var s = File.OpenWrite($"{r.GetId()}.json"))
                using (var ss = new StreamWriter(s))
                {
                    Google.Protobuf.JsonFormatter.Default.WriteValue(ss, r);
                }

            }
            else
            {
                using (var s = File.OpenWrite($"{r.GetId()}.hkpv"))
                {
                    Google.Protobuf.MessageExtensions.WriteTo(r, s);
                }
            }
        }
    }

}
