using DbUp.Engine.Output;
using Microsoft.Extensions.Logging;

namespace Vodamep.Api.Engines.SqlServer
{
    public class DbUpLogWrapper : IUpgradeLog
    {
        private readonly ILogger _logger;

        public DbUpLogWrapper(ILogger logger)
        {
            _logger = logger;
        }
        public void WriteError(string format, params object[] args)
        {
            _logger.LogError(format, args);
        }

        public void WriteInformation(string format, params object[] args)
        {
            _logger.LogInformation(format, args);
        }

        public void WriteWarning(string format, params object[] args)
        {
            _logger.LogWarning(format, args);
        }
    }
}
