using FluentValidation.Results;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    public class HkpvReportValidationResultFormatter
    {
        public string Format(HkpvReport report, ValidationResult validationResult)
        {
            if (validationResult.IsValid)
                return string.Empty;

            var result = new StringBuilder();

            result.AppendLine($"<h3>Fehlerliste Datenmeldung {report.Institution?.Name}, {report.From?.ToString("dd.MM.yyyy")}-{report.To?.ToString("dd.MM.yyyy")}</h3>");

            result.AppendLine("<table>");
            result.AppendLine("<tbody>");

            var entries = validationResult.Errors.Select(x => new
            {
                Info = this.GetInfo(report, x.PropertyName),
                Message = x.ErrorMessage,
                Value = x.AttemptedValue?.ToString()
            }).ToArray();

            foreach (var groupedInfos in entries.OrderBy(x => x.Info).GroupBy(x => x.Info))
            {
                result.Append($"<tr><td>{groupedInfos.Key}</td><td>{groupedInfos.First().Message}</td><td>{groupedInfos.First().Value}</td></tr>");

                foreach (var info in groupedInfos.Skip(1))
                {
                    result.Append($"<tr><td></td><td>{info.Message}</td><td>{info.Value}</td></tr>");
                }

            }
            result.AppendLine("</tbody>");
            result.AppendLine("</table>");

            return result.ToString();
        }

        private static string GetIdPattern(string propertyName) => $@"{propertyName}\[(?<id>\d+)\]";

        private static string NewLine => "</br>";

        private readonly GetNameByPatternStrategy[] _strategies = new[]
        {
            new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.Persons)), GetNameOfPerson),
            new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.PersonalData)), GetNameOfPerson),
            new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.Staffs)), GetNameOfStaff),
            new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.Activities)), GetNameOfActivity),
            new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.OtherActivities)), GetNameOfOtherActivity),
            new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.Consultations)), GetNameOfConsultation),
        };

        private static string GetNameOfPerson(HkpvReport report, int index)
        {
            if (report.Persons.Count > index && index >= 0)
            {
                var e = report.PersonalData[index];
                return $"Person: {e.FamilyName} {e.GivenName}";
            }

            return string.Empty;
        }
        private static string GetNameOfStaff(HkpvReport report, int index)
        {
            if (report.Staffs.Count > index && index >= 0)
            {
                var e = report.Staffs[index];
                return $"Mitarbeiter: {e.FamilyName} {e.GivenName}";
            }

            return string.Empty;
        }

        private static string GetNameOfActivity(HkpvReport report, int index)
        {
            if (report.Activities.Count > index && index >= 0)
            {
                var e = report.Activities[index];
                return $"Aktivität {e.Date.ToString("dd.MM.yyyy")}{NewLine}{e.Type}{NewLine}{GetNameOfPersonById(report, e.PersonId)}{NewLine}{GetNameOfStaffById(report, e.StaffId)}";
            }

            return string.Empty;
        }

        private static string GetNameOfPersonById(HkpvReport report, string id)
        {
            var e = report.PersonalData.Where(x => x.Id == id).FirstOrDefault();

            if (e == null)
                return string.Empty;

            return $"{e.FamilyName} {e.GivenName}";
        }

        private static string GetNameOfStaffById(HkpvReport report, string id)
        {
            var e = report.Staffs.Where(x => x.Id == id).FirstOrDefault();

            if (e == null)
                return string.Empty;

            return $"{e.FamilyName} {e.GivenName}";
        }

        private static string GetNameOfOtherActivity(HkpvReport report, int id)
        {
            if (report.OtherActivities.Count > id && id >= 0)
            {
                var e = report.OtherActivities[id];
                return $"Aktivität anderer Verein {e.Date.ToString("dd.MM.yyyy")}: {GetNameOfPersonById(report, e.PersonId)} {GetNameOfStaffById(report, e.StaffId)}";
            }

            return string.Empty;
        }

        private static string GetNameOfConsultation(HkpvReport report, int id)
        {
            if (report.Consultations.Count > id && id >= 0)
            {
                var e = report.Consultations[id];
                return $"Beratung {e.Date.ToString("dd.MM.yyyy")}, {e.Type}: {GetNameOfStaffById(report, e.StaffId)}";
            }

            return string.Empty;
        }

        private string GetInfo(HkpvReport report, string propertyName)
        {
            foreach (var strategy in _strategies)
            {
                var r = strategy.GetInfo(report, propertyName);

                if (!string.IsNullOrEmpty(r)) return r;
            }
            return propertyName;
        }

        private class GetNameByPatternStrategy
        {
            private readonly Regex _pattern;
            private readonly Func<HkpvReport, int, string> _resolveInfo;

            public GetNameByPatternStrategy(string pattern, Func<HkpvReport, int, string> resolveInfo)
            {
                _pattern = new Regex(pattern);
                _resolveInfo = resolveInfo;
            }

            public string GetInfo(HkpvReport report, string propertyName)
            {
                var m = this._pattern.Match(propertyName);

                if (m.Success && int.TryParse(m.Groups["id"].Value, out int id))
                    return _resolveInfo(report, id);

                return string.Empty;

            }
        }


    }
}
