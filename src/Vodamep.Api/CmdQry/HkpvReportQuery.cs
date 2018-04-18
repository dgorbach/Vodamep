using Vodamep.Hkpv.Model;

namespace Vodamep.Api.CmdQry
{
    public class HkpvReportQuery : IQuery<HkpvReport>
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string Institution { get; set; }
    }
}
