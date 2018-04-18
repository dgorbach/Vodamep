namespace Vodamep.Api.CmdQry
{
    public interface IEngine
    {
        void Execute(ICommand save);

        QueryResult<T> Query<T>(IQuery<T> query);
    }
}
