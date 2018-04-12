using FluentValidation;
using PowerArgs;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Vodamep.Data;
using Vodamep.Data.Dummy;
using Vodamep.Hkpv.Model;
using Vodamep.Hkpv.Validation;

namespace Vodamep.Client
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    [ArgDescription("(dmc) Daten-Meldungs-Client:")]
    public class VodamepProgram
    {
        [ArgActionMethod, ArgDescription("Validate a file.")]
        public void Validate(ValidateArgs args)
        {
            var isGerman = Thread.CurrentThread.CurrentCulture.Name.StartsWith("de", StringComparison.CurrentCultureIgnoreCase);
            if (isGerman)
            {
                var loc = new DisplayNameResolver();
                ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
            }


            var report = Read(args.File);

            var validator = new HkpvReportValidator();

            var result = validator.Validate(report);

            if (result.IsValid)
                Console.WriteLine("ok");
            else
            {
                var formatter = new HkpvReportValidationResultFormatter(ResultFormatterTemplate.Text);
                var message = formatter.Format(report, result);
                Console.WriteLine(message);
            }
        }


        [ArgActionMethod, ArgDescription("Pack a file.")]
        public void PackFile(PackFileArgs args)
        {
            var report = Read(args.File);

            var file = report.WriteToFile(args.Json);

            Console.WriteLine($"{file} created");
        }

        [ArgActionMethod, ArgDescription("Pack some random data.")]
        public void PackRandom(PackRandomArgs args)
        {
            int? year = args.Year;
            if (year < 2000 || year > DateTime.Today.Year) year = null;

            int? month = args.Month;
            if (month < 1 || month > 12) month = null;

            var r = DataGenerator.Instance.CreateHkpvReport(year, month, args.Persons, args.Staffs, args.AddActivities);

            var file = r.WriteToFile(args.Json);

            Console.WriteLine($"{file} created");
        }

        [ArgActionMethod, ArgDescription("Get a list af valid values.")]
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


        private HkpvReport Read(string file)
        {
            var content = File.ReadAllBytes(file);

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

            return r;
        }
    }

}
