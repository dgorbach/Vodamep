using DbUp;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Vodamep.Api.Engines.SqlServer
{
    public class Upgrader
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public Upgrader(string connectionString, ILogger logger)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        public void Upgrade()
        {
            var upgrader = DeployChanges.To
                .SqlDatabase(_connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), x => x.StartsWith($"{typeof(Upgrader).Namespace}.Scripts"))
                .LogTo(new DbUpLogWrapper(_logger))
                .Build();

            var result = upgrader.PerformUpgrade();
        }
    }
}
