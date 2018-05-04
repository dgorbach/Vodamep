using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Vodamep.Hkpv.Validation;

namespace Vodamep.Hkpv.Model
{
    public static class HkpvReportExtensions
    {
        public static HkpvReport AddPerson(this HkpvReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static HkpvReport AddPersons(this HkpvReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        private static HkpvReport InvokeAndReturn(this HkpvReport m, Action<HkpvReport> action)
        {
            action(m);
            return m;
        }

        public static Task<SendResult> Send(this HkpvReport report, Uri address, string username, string password) => new HkpvReportSendClient(address).Send(report, username, password);

        public static HkpvReportValidationResult Validate(this HkpvReport report) => (HkpvReportValidationResult)new HkpvReportValidator().Validate(report);

        public static string ValidateToText(this HkpvReport report) => new HkpvReportValidationResultFormatter(ResultFormatterTemplate.Text).Format(report, Validate(report));

        public static HkpvReport AsSorted(this HkpvReport report)
        {
            var result = new HkpvReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To
            };

            result.Activities.AddRange(report.Activities.OrderBy(x => x));
            result.Consultations.AddRange(report.Consultations.OrderBy(x => x));

            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));            
            result.Staffs.AddRange(report.Staffs.OrderBy(x => x.Id));

            return result;
        }

        public static string GetSHA256Hash(this HkpvReport report)
        {
            using (var s = SHA256.Create())
            {
                var h = s.ComputeHash(report.ToByteArray());

                var sha256 = System.Net.WebUtility.UrlEncode(Convert.ToBase64String(h));

                return sha256;
            }
        }
    }
}