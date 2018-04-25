using DbUp;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;

namespace Vodamep.Api.Engines.SqlServer
{
    public class DbUpdater
    {
        private readonly ILogger _logger;
        private readonly HashSet<int> done = new HashSet<int>();
        private readonly object l = new object();

        public DbUpdater(ILogger<DbUpdater> logger)
        {
            _logger = logger;
        }

        public void Upgrade(string connectionString)
        {
            var key = connectionString.ToLower().GetHashCode();

            if (!done.Contains(key))
            {
                lock (l)
                {
                    if (done.Contains(key))
                        return;

                    done.Add(key);
                    var upgrader = DeployChanges.To
                        .SqlDatabase(connectionString)
                        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), x => x.StartsWith($"{typeof(DbUpdater).Namespace}.Scripts"))
                        .LogTo(new DbUpLogWrapper(_logger))
                        .Build();

                    var result = upgrader.PerformUpgrade();
                }
            }
        }
    }
}
