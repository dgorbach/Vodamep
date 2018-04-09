using FluentValidation.Results;
using System;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    public class ResultFormatterTemplate
    {
        public Func<HkpvReport, ValidationResult, string> Header { get; set; }

        public Func<HkpvReport, ValidationResult, string> Footer { get; set; }
        public Func<(string Info, string Message, string Value), string> FirstLine { get; set; }

        public Func<(string Message, string Value), string> Line { get; set; }

        public string Linefeed { get; set; }


        public static ResultFormatterTemplate HTML = new ResultFormatterTemplate()
        {
            Header = (report, vr) => $"<h3>Fehlerliste Datenmeldung {report.Institution?.Name}, {report.FromD.ToString("dd.MM.yyyy")}-{report.ToD.ToString("dd.MM.yyyy")}</h3>",
            Footer = (report, vr) => "</tbody></table>",
            FirstLine = (x) => $"<tr><td>{x.Info}</td><td>{x.Message}</td><td>{x.Value}</td></tr>",
            Line = (x) => $"<tr><td></td><td>{x.Message}</td><td>{x.Value}</td></tr>",
            Linefeed = "<br/>"
        };

        public static ResultFormatterTemplate Text = new ResultFormatterTemplate()
        {
            Header = (report, vr) => $"# Fehlerliste Datenmeldung {report.Institution?.Name}, {report.FromD.ToString("dd.MM.yyyy")}-{report.ToD.ToString("dd.MM.yyyy")}",
            Footer = (report, vr) => "",
            FirstLine = (x) => $"{new string('-', 30)}{System.Environment.NewLine}{x.Info}{System.Environment.NewLine}\t- {x.Message}{(!string.IsNullOrEmpty(x.Value) ? $" ({x.Value})": "")}",
            Line = (x) => $"\t- {x.Message}",
            Linefeed = System.Environment.NewLine
        };
    }
}
