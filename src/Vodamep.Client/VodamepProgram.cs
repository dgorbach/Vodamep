using PowerArgs;
using System;
using Vodamep.Data;
using Vodamep.Data.Dummy;
using Vodamep.Hkpv;
using Vodamep.Hkpv.Model;
using Vodamep.Hkpv.Validation;

namespace Vodamep.Client
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    [ArgDescription("(dmc) Daten-Meldungs-Client:")]
    public class VodamepProgram
    {

        private void HandleFailure(string message = null)
        {
            throw new Exception(message);            
        }

        [ArgActionMethod, ArgDescription("Send a file to an endpoint.")]
        public void Send(SendArgs args)
        {
            var report = ReadReport(args.File);

            var sendResult = report.Send(new Uri(args.Address), args.User, args.Pwd).Result;

            if (!string.IsNullOrEmpty(sendResult?.Message))
            {
                Console.WriteLine(sendResult.Message);
            }

            if (!string.IsNullOrEmpty(sendResult?.ErrorMessage))
            {
                Console.WriteLine(sendResult.ErrorMessage);
            }

            if (!(sendResult?.IsValid ?? false))
            {
                HandleFailure("send failed.");
            }
            else
            {
                Console.WriteLine("send succeeded.");
            }
        }

        [ArgActionMethod, ArgDescription("Validate a file.")]
        public void Validate(ValidateArgs args)
        {
            var report = ReadReport(args.File);

            var result = report.Validate();

            var formatter = new HkpvReportValidationResultFormatter(ResultFormatterTemplate.Text);
            var message = formatter.Format(report, result);
            Console.WriteLine(message);

            if (!result.IsValid)
            {
                HandleFailure();
            }
        }


        [ArgActionMethod, ArgDescription("Pack a file.")]
        public void PackFile(PackFileArgs args)
        {
            var report = ReadReport(args.File);

            var file = report.WriteToPath("", asJson: args.Json, compressed: !args.NoCompression);

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

            var file = r.WriteToPath("", asJson: args.Json, compressed: !args.NoCompression);

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
                case ListSources.Postcode_City:
                    provider = Postcode_CityProvider.Instance;
                    break;
                default:
                    HandleFailure($"Provider '{args.Source}' not implemented.");
                    return;
            }

            foreach (var line in provider?.GetCSV())
                Console.WriteLine(line);

        }


        private HkpvReport ReadReport(string file)
        {
            try
            {
                var report = new HkpvReportSerializer().DeserializeFile(file);
                return report;
            }
            catch
            {

            }

            HandleFailure("Unable to read report.");
            return null;
        }
    }

}
