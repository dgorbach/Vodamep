using Bazinga.AspNetCore.Authentication.Basic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Vodamep.Api.CmdQry;
using Vodamep.Api.Engines.FileSystem;
using Vodamep.Api.Engines.SqlServer;

namespace Vodamep.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this._configuration = configuration;
            this._loggerFactory = loggerFactory;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var verifiery = new CredentialVerifier();

            services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                .AddBasicAuthentication(verifiery.Verify);

            services.AddRouting();

            this.ConfigureEngine(services);

            services.AddTransient<VodamepHandler>();
            services.AddSingleton<DbUpdater>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            app.UseVodamep();
        }

        private void ConfigureEngine(IServiceCollection services)
        {
            var sqlEngineConfig = new SqlServerEngineConfiguration();
            this._configuration.Bind(nameof(SqlServerEngine), sqlEngineConfig);

            if (!string.IsNullOrEmpty(sqlEngineConfig.ConnectionString))
            {
                _loggerFactory.CreateLogger<Startup>().LogInformation("Using SqlServerEngine");

                services.AddTransient<Func<IEngine>>(sp => () => new SqlServerEngine(sqlEngineConfig, sp.GetService<DbUpdater>(), sp.GetService<ILogger<SqlServerEngine>>()));
                return;
            }

            var fileEngineConfig = new FileEngineConfiguration();
            this._configuration.Bind(nameof(FileEngine), fileEngineConfig);

            if (!string.IsNullOrEmpty(fileEngineConfig.Path))
            {
                _loggerFactory.CreateLogger<Startup>().LogInformation("Using FileEngine: '{path}'", fileEngineConfig.Path);
                services.AddTransient<Func<IEngine>>(sp => () => new FileEngine(fileEngineConfig, sp.GetService<ILogger<FileEngine>>()));
                return;
            }

            _loggerFactory.CreateLogger<Startup>().LogError("No Engine Configured");
            throw new Exception("No Engine Configured");
        }
    }
}
