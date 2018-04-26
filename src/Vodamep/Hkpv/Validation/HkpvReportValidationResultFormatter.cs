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

        private readonly ResultFormatterTemplate _template;

        public HkpvReportValidationResultFormatter(ResultFormatterTemplate template)
        {
            _template = template;

            _strategies = new[]
            {
                new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.Persons)), GetNameOfPerson),
                new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.PersonalData)), GetNameOfPerson),
                new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.Staffs)), GetNameOfStaff),
                new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.Activities)), GetNameOfActivity),                
                new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.Consultations)), GetNameOfConsultation),

                new GetNameByPatternStrategy($"^{nameof(HkpvReport.To)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(HkpvReport.ToD)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(HkpvReport.From)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(HkpvReport.FromD)}$",(a,b) => string.Empty),
            };

        }

        public string Format(HkpvReport report, ValidationResult validationResult)
        {
            if (!validationResult.Errors.Any())
                return string.Empty;

            var result = new StringBuilder();

            result.Append(_template.Header(report, validationResult));

            var severities = validationResult.Errors.OrderBy(x => x.Severity).GroupBy(x => x.Severity);
            foreach (var severity in severities)
            {
                result.Append(_template.HeaderSeverity(GetSeverityName(severity.Key)));

                var entries = severity.Select(x => new
                {
                    Info = this.GetInfo(report, x.PropertyName),
                    Message = x.ErrorMessage,
                    Value = x.AttemptedValue?.ToString()
                }).ToArray();

                foreach (var groupedInfos in entries.OrderBy(x => x.Info).GroupBy(x => x.Info))
                {
                    result.Append(_template.FirstLine((groupedInfos.Key, groupedInfos.First().Message, groupedInfos.First().Value)));

                    foreach (var info in groupedInfos.Skip(1))
                    {
                        result.Append(_template.Line((info.Message, info.Value)));
                    }
                }

                result.Append(_template.FooterSeverity(severity.ToString()));
            }
            return result.ToString();
        }

        private static string GetIdPattern(string propertyName) => $@"{propertyName}\[(?<id>\d+)\]";


        private string GetSeverityName(FluentValidation.Severity severity)
        {
            switch (severity)
            {
                case FluentValidation.Severity.Error:
                    return "Fehler";
                case FluentValidation.Severity.Warning:
                    return "Warnung";
                case FluentValidation.Severity.Info:
                    return "Information";
                default:
                    return severity.ToString();
            }            
        }

        private readonly GetNameByPatternStrategy[] _strategies;

        private string GetNameOfPerson(HkpvReport report, int index)
        {
            if (report.Persons.Count > index && index >= 0)
            {
                var e = report.PersonalData[index];
                return $"Person: {e.FamilyName} {e.GivenName}";
            }

            return string.Empty;
        }
        private string GetNameOfStaff(HkpvReport report, int index)
        {
            if (report.Staffs.Count > index && index >= 0)
            {
                var e = report.Staffs[index];
                return $"Mitarbeiter: {e.FamilyName} {e.GivenName}";
            }

            return string.Empty;
        }

        private string GetNameOfActivity(HkpvReport report, int index)
        {
            if (report.Activities.Count > index && index >= 0)
            {
                var e = report.Activities[index];
                return $"Aktivität {e.DateD.ToString("dd.MM.yyyy")}{_template.Linefeed}  {e.Type}{_template.Linefeed}  {GetNameOfPersonById(report, e.PersonId)}{_template.Linefeed}  {GetNameOfStaffById(report, e.StaffId)}";
            }

            return string.Empty;
        }

        private string GetNameOfPersonById(HkpvReport report, string id)
        {
            var e = report.PersonalData.Where(x => x.Id == id).FirstOrDefault();

            if (e == null)
                return string.Empty;

            return $"{e.FamilyName} {e.GivenName}";
        }

        private string GetNameOfStaffById(HkpvReport report, string id)
        {
            var e = report.Staffs.Where(x => x.Id == id).FirstOrDefault();

            if (e == null)
                return string.Empty;

            return $"{e.FamilyName} {e.GivenName}";
        }


        private string GetNameOfConsultation(HkpvReport report, int id)
        {
            if (report.Consultations.Count > id && id >= 0)
            {
                var e = report.Consultations[id];
                return $"Beratung {e.DateD.ToString("dd.MM.yyyy")}, {e.Type}: {GetNameOfStaffById(report, e.StaffId)}";
            }

            return string.Empty;
        }

        private string GetInfo(HkpvReport report, string propertyName)
        {
            foreach (var strategy in _strategies)
            {
                var r = strategy.GetInfo(report, propertyName);

                if (r.Success) return r.Info;
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

            public (bool Success, string Info) GetInfo(HkpvReport report, string propertyName)
            {
                var m = this._pattern.Match(propertyName);

                if (m.Success)
                {
                    int.TryParse(m.Groups["id"].Value, out int id);

                    return (true, _resolveInfo(report, id));
                }
                return (false, string.Empty);
            }
        }
    }
}
