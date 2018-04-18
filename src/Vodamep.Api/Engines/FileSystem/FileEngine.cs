using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Vodamep.Api.CmdQry;
using Vodamep.Hkpv;
using Vodamep.Hkpv.Model;

namespace Vodamep.Api.Engines.FileSystem
{

    public class FileEngine : IEngine
    {
        private readonly string _path;
        private readonly ILogger<FileEngine> _logger;

        public FileEngine(string path, ILogger<FileEngine> logger)
        {
            this._path = path;
            this._logger = logger;

            _logger?.LogInformation("Uses Directory {path}", path);

            if (!Directory.Exists(_path))
            {
                _logger?.LogInformation("Creates Directory {path}", path);
                Directory.CreateDirectory(_path);
            }
        }
        public void Execute(ICommand cmd)
        {
            switch (cmd)
            {
                case HkpvReportSaveCommand save:
                    this.Save(save.Report);
                    return;
            }
        }

        public QueryResult<T> Query<T>(IQuery<T> query)
        {
            QueryResult<T> result = null;

            switch (query)
            {
                case HkpvReportQuery q:
                    result = new QueryResult<T>() { Result = this.Query(q).Cast<T>().ToArray() };
                    break;
                case HkpvReportInfoQuery q:
                    result = new QueryResult<T>() { Result = this.Query(q).Cast<T>().ToArray() };
                    break;
            }

            return result;
        }

        private void Save(HkpvReport report)
        {
            var (info, lastFilename) = GetLastFile(report.Institution.Id);

            if (info != null && string.Equals(GetFilename(report, info.Id), lastFilename, StringComparison.CurrentCultureIgnoreCase))
            {
                _logger.LogInformation("Report already exits. Skip saving.");
                return;
            }

            var filename = Path.Combine(_path, GetFilename(report, (info?.Id ?? 0) + 1));

            report.WriteToFile(filename, asJson: false, compressed: true);

            _logger.LogInformation("Report saved: {filename}", filename);
        }

        private static Regex _filenamePattern = new Regex(@"^(?<id>\d+)__(?<institution>.+?)_(?<year>\d+)_(?<month>\d+)_(?<hash>.+?)\.(zip|hkpv|json)$");

        private string GetFilename(HkpvReport report, int id) => $"{id:00000000}__{HkpvReportSerializer.GetFileName(report, false, true)}";

        private (HkpvReportInfo info, string filename) GetLastFile(string institution)
        {
            var currentId = Directory.GetFiles(_path).OrderByDescending(x => x)
                .Select(x => Path.GetFileName(x))
                .Select(x => new { Filename = x, Match = _filenamePattern.Match(x) })
                .Where(x => x.Match.Success)
                .Where(x => string.IsNullOrEmpty(institution) || string.Equals(x.Match.Groups["institution"].Value, institution, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            if (currentId != null)
            {
                var info = new HkpvReportInfo()
                {
                    Id = int.Parse(currentId.Match.Groups["id"].Value),
                    Institution = currentId.Match.Groups["institution"].Value,
                    Year = int.Parse(currentId.Match.Groups["year"].Value),
                    Month = int.Parse(currentId.Match.Groups["month"].Value),
                    Hash = currentId.Match.Groups["hash"].Value
                };

                return (info, currentId.Filename);
            }

            return (null, string.Empty);
        }

        private IEnumerable<HkpvReport> Query(HkpvReportQuery qry)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<HkpvReportInfo> Query(HkpvReportInfoQuery qry)
        {
            throw new NotImplementedException();
        }
    }
}
