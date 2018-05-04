using System;
using System.IO;
using Vodamep.Data.Dummy;
namespace Vodamep.Hkpv.Model
{
    public partial class HkpvReport
    {
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsValue(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsValue(); }


        public static HkpvReport CreateDummyData()
        {
            var r = new HkpvReport()
            {
                Institution = new Institution()
                {
                    Id = "test",
                    Name = "Test"
                }
            };

            r.FromD = DateTime.Today.FirstDateInMonth().AddMonths(-1);
            r.ToD = r.FromD.LastDateInMonth();

            r.AddDummyPerson();
            r.AddDummyStaff();
            r.AddDummyActivities();

            return r;

        }

        public static HkpvReport ReadFile(string file)
        {
            var report = new HkpvReportSerializer().DeserializeFile(file);
            return report;
        }

        public static HkpvReport Read(byte[] data)
        {
            var report = new HkpvReportSerializer().Deserialize(data);
            return report;
        }

        public static HkpvReport Read(Stream data)
        {
            var report = new HkpvReportSerializer().Deserialize(data);
            return report;
        }

        public string WriteToPath(string path, bool asJson = false, bool compressed = true) => new HkpvReportSerializer().WriteToPath(this, path, asJson, compressed);

        public void WriteToFile(string filename, bool asJson = false, bool compressed = true) => new HkpvReportSerializer().WriteToFile(this, filename, asJson, compressed);

        public MemoryStream WriteToStream(bool asJson = false, bool compressed = true) => new HkpvReportSerializer().WriteToStream(this, asJson, compressed);

    }
}