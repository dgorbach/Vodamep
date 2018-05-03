using FluentValidation.Results;
using System;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class ResultFormatterTemplate
    {
        public Func<HkpvReport, ValidationResult, string> Header { get; set; }

        public Func<HkpvReport, ValidationResult, string> Footer { get; set; }

        public Func<string, string> HeaderSeverity { get; set; }

        public Func<string, string> FooterSeverity { get; set; }

        public Func<(string Info, string Message, string Value), string> FirstLine { get; set; }

        public Func<(string Message, string Value), string> Line { get; set; }

        public string Linefeed { get; set; }


        public static ResultFormatterTemplate HTML = new ResultFormatterTemplate()
        {
            Header = (report, vr) => $"<h3>Validierung Datenmeldung {report.Institution?.Name}, {report.FromD.ToString("dd.MM.yyyy")}-{report.ToD.ToString("dd.MM.yyyy")}</h3>{System.Environment.NewLine}",
            Footer = (report, vr) => "",
            HeaderSeverity = (s) => $"<h4>{s}</h4><table><tbody>{System.Environment.NewLine}",
            FooterSeverity = (s) => "</tbody></table>{System.Environment.NewLine}",
            FirstLine = (x) => $"<tr><td>{x.Info}</td><td>{x.Message}</td><td>{x.Value}</td></tr>{System.Environment.NewLine}",
            Line = (x) => $"<tr><td></td><td>{x.Message}</td><td>{x.Value}</td></tr>{System.Environment.NewLine}",
            Linefeed = "<br/>"
        };

        public static ResultFormatterTemplate Text = new ResultFormatterTemplate()
        {
            Header = (report, vr) => $"# Validierung Datenmeldung {report.Institution?.Name}, {report.FromD.ToString("dd.MM.yyyy")}-{report.ToD.ToString("dd.MM.yyyy")}{System.Environment.NewLine}",
            Footer = (report, vr) => "",
            HeaderSeverity = (s) => $"{s}{System.Environment.NewLine}",
            FooterSeverity = (s) => "",
            FirstLine = (x) => $"{new string('-', 30)}{System.Environment.NewLine}{x.Info}{System.Environment.NewLine}\t- {x.Message}{(!string.IsNullOrEmpty(x.Value) ? $" ({x.Value})": "")}{System.Environment.NewLine}",
            Line = (x) => $"\t- {x.Message}{System.Environment.NewLine}",
            Linefeed = System.Environment.NewLine
        };
    }
}
