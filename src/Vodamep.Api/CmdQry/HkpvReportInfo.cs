using System;

namespace Vodamep.Api.CmdQry
{
    public class HkpvReportInfo
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Institution { get; set; }
        public string Hash { get; set; }        

        public DateTime Created { get; set; }
    }
}
