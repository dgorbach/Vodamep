using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Vodamep.Hkpv.Model
{
    public static class HkpvReportExtensions
    {

        public static HkpvReport AddPerson(this HkpvReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static HkpvReport AddPersons(this HkpvReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        public static HkpvReport AddPerson(this HkpvReport report, (Person Person, PersonalData Data) p)
        {
            if (p.Person != null)
                report.Persons.Add(p.Person);

            if (p.Data != null)
                report.PersonalData.Add(p.Data);

            return report;
        }

        public static HkpvReport AddPersons(this HkpvReport report, (Person Person, PersonalData Data)[] ps)
        {
            foreach (var p in ps)
            {
                if (p.Person != null)
                    report.Persons.Add(p.Person);

                if (p.Data != null)
                    report.PersonalData.Add(p.Data);
            }

            return report;
        }

        private static HkpvReport InvokeAndReturn(this HkpvReport m, Action<HkpvReport> action)
        {
            action(m);
            return m;
        }

        public static HkpvReport Read(string file)
        {
            byte[] content;
            if (file.EndsWith(".zip", StringComparison.CurrentCultureIgnoreCase))
            {
                using (var archive = ZipFile.OpenRead(file))
                {
                    using (var ms = new MemoryStream())
                    {
                        archive.Entries.First().Open().CopyTo(ms);
                        content = ms.ToArray();
                    };
                }
            }
            else
            {
                content = File.ReadAllBytes(file);
            }

            var isJson = System.Text.Encoding.UTF8.GetString(content.Take(5).ToArray()).TrimStart().StartsWith("{");

            HkpvReport r;

            if (isJson)
            {
                var json = System.Text.Encoding.UTF8.GetString(content);

                r = HkpvReport.Parser.ParseJson(json);

            }
            else
            {
                r = HkpvReport.Parser.ParseFrom(content);
            }

            return r;
        }

        public static string WriteToFile(this HkpvReport report, bool asJson, string path = "", bool compressed = true)
        {
            string filename;

            report = report.AsSorted();
            using (MemoryStream ms = new MemoryStream())
            {
                Stream saveStream = ms;

                if (asJson)
                {
                    filename = Path.Combine(path, $"{report.GetId()}.json");
                    using (var ss = new StreamWriter(ms))
                    {
                        Google.Protobuf.JsonFormatter.Default.WriteValue(ss, report);
                        ss.Flush();
                        ms.Position = 0;
                        filename = Save(ms, filename, compressed);
                    }
                }
                else
                {
                    filename = Path.Combine(path, $"{report.GetId()}.hkpv");
                    Google.Protobuf.MessageExtensions.WriteTo(report, ms);
                    ms.Position = 0;
                    filename = Save(ms, filename, compressed);
                }                
            }

            return filename;
        }

        private static string Save(Stream ms, string filename, bool compressed)
        {
            
            if (compressed)
            {
                var zipFileName = $"{filename.Substring(0, filename.LastIndexOf('.'))}.zip";
                using (FileStream zipToOpen = new FileStream(zipFileName, FileMode.Create))
                using (ZipArchive arch = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {

                    var zipEntry = arch.CreateEntry(filename);
                    using (var zipStream = zipEntry.Open())
                    {
                        ms.CopyTo(zipStream);
                    }
                }


                filename = zipFileName;

            }
            else
            {
                using (var s = File.OpenWrite(filename))
                {
                    ms.CopyTo(s);
                }
            }


            return filename;
        }
    }
}