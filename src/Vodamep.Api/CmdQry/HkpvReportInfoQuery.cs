namespace Vodamep.Api.CmdQry
{
    public class HkpvReportInfoQuery : IQuery<HkpvReportInfo>
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string Institution { get; set; }
    }
}
