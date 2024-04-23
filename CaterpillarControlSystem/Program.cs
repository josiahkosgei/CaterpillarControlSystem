

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

var host = StartupCofiguration();

// bootstrap host configs
static IHost StartupCofiguration()
{
    var builder = new ConfigurationBuilder();

    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    // setup serilog, reads Logger Configuration from appsettings.json, and creates logger
    Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Build())
                    .CreateLogger();
    Log.Logger.Information("starting serilog in a Caterpillar Control System...");
    // Set serilog as logging Provider
    var host = Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {

        })
        .UseSerilog((context, configuration) =>
        {
            var config = context.Configuration;
            configuration.ReadFrom.Configuration(config);
        })
        .Build();

    return host;
}