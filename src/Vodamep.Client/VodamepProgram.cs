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

        [ArgActionMethod, ArgDescription("Absenden der Meldung.")]
        public void Send(SendArgs args)
        {
            var report = ReadReport(args.File);

            var address = args.Address.Trim();

            address = address.EndsWith(@"\") ? address : $"{address}/";

            var sendResult = report.Send(new Uri(address), args.User, args.Password).Result;

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
                HandleFailure("Fehlgeschlagen");
            }
            else
            {
                Console.WriteLine("Erfolgreich");
            }
        }

        [ArgActionMethod, ArgDescription("Prüfung der Meldung.")]
        public void Validate(ValidateArgs args)
        {
            var report = ReadReport(args.File);

            var result = report.Validate();

            var formatter = new HkpvReportValidationResultFormatter(ResultFormatterTemplate.Text);
            var message = formatter.Format(report, result);

            if (!result.IsValid)
            {
                HandleFailure(message);
            }
            else if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("ok");
            }
        }


        [ArgActionMethod, ArgDescription("Meldung neu verpacken.")]
        public void PackFile(PackFileArgs args)
        {
            var report = ReadReport(args.File);

            var file = report.WriteToPath("", asJson: args.Json, compressed: !args.NoCompression);

            Console.WriteLine($"{file} wurde erzeugt.");
        }

        [ArgActionMethod, ArgDescription("Meldung mit Testdaten erzeugen.")]
        public void PackRandom(PackRandomArgs args)
        {
            int? year = args.Year;
            if (year < 2000 || year > DateTime.Today.Year) year = null;

            int? month = args.Month;
            if (month < 1 || month > 12) month = null;

            var r = DataGenerator.Instance.CreateHkpvReport(year, month, args.Persons, args.Staffs, args.AddActivities);

            var file = r.WriteToPath("", asJson: args.Json, compressed: !args.NoCompression);

            Console.WriteLine($"{file} wurde erzeugt.");
        }

        [ArgActionMethod, ArgDescription("Listet erlaubte Werte.")]
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
                    HandleFailure($"Source '{args.Source}' not implemented.");
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

            HandleFailure("Daten konnten nicht gelesen werden.");
            return null;
        }
    }

}
