using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OMI.API
{
#pragma warning disable CS1591
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();

      await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
              var env = hostingContext.HostingEnvironment;

              config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                  .AddJsonFile($"appsettings.Local.json", optional: true, reloadOnChange: true);

              if (env.IsDevelopment())
              {
                var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                if (appAssembly != null)
                {
                  config.AddUserSecrets(appAssembly, optional: true);
                }
              }

              config.AddEnvironmentVariables();

              if (args != null)
              {
                config.AddCommandLine(args);
              }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>()
              .ConfigureLogging((hostingContext, logging) =>
              {
                var loggingOptions = hostingContext.Configuration.GetSection("Log4NetCore")
                                               .Get<Log4NetProviderOptions>();
                logging.AddLog4Net(loggingOptions);
                logging.SetMinimumLevel(LogLevel.Debug);
              });
            });
  }
#pragma warning restore CS1591
}
