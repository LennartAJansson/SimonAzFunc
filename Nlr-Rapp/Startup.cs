using System;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Nlr_Rapp.SharePoint;
using Nlr_Rapp.SharePoint.Settings;

[assembly: FunctionsStartup(typeof(Nlr_Rapp.Startup))]
namespace Nlr_Rapp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Environment.CurrentDirectory)
              .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();

            builder.Services.Configure<AppSettings>(configuration.GetSection("SharePoint"));
            builder.Services.AddTransient<GraphServiceClientProvider>();
        }
    }
}
