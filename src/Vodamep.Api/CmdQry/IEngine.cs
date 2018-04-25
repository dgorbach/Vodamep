using System.Security.Principal;

namespace Vodamep.Api.CmdQry
{
    public interface IEngine
    {
        void Login(IPrincipal principal);

        void Execute(ICommand cmd);

        //hier könnte auch Funktionalität zur Datenabfrage sein: QueryResult<T> Query<T>(IQuery<T> query);
    }

    /*
     * 
     * 
     * 
    public class HkpvReportInfoQuery : IQuery<HkpvReportInfo>
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string Institution { get; set; }
    }

    public interface IQuery<T>  
    {    
    }
     
    public class QueryResult<T>
    {
        public T[] Result { get; set; }
    }

    public class HkpvReportQuery : IQuery<HkpvReport>
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string Institution { get; set; }
    }
     
     */
}
