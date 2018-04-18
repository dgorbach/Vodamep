using Vodamep.Hkpv.Model;

namespace Vodamep.Api.CmdQry
{
    public class HkpvReportSaveCommand : ICommand
    {
        public HkpvReport Report { get; set; }
    }
}
