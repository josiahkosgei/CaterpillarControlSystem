using CaterpillarControlSystem.App;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

IHost host = StartupCofiguration();

// Instantiate Caterpillar class
var caterpillar = new Caterpillar();

// Read rider commands while no obstacle is encountered
bool obstacleEncountered = false;
while (!obstacleEncountered)
{
    caterpillar.DisplayRadarImageGrid();

    Console.WriteLine();
    Console.Write("Enter a command (U: Move Up | D: Move Down | L: Move Left | R: Move Right | G: Grow Caterpillar | S: Shrink Caterpillar): ");
    string riderCommand = Console.ReadKey().KeyChar.ToString().ToUpper();
    int steps = 0;

    //Steps are not required when the commands are either Grow or Shrink
    if (!string.IsNullOrEmpty(riderCommand) && !(riderCommand.Equals("G") || riderCommand.Equals("S")))
    {

        Console.WriteLine();
        Console.Write("Enter a number of steps to be taken: ");
        steps = Convert.ToInt32(Console.ReadLine());
    }

    Console.WriteLine();

    // Call Caterpiller to execute commands and steps
    caterpillar.ExecuteRiderCommand(char.Parse(riderCommand), steps);
}

// bootstrap host configs
static IHost StartupCofiguration()
{
    var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

    // setup serilog, reads Logger Configuration from appsettings.json, and creates logger
    var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();



    // Set serilog as logging Provider
    var host = Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
            services.AddLogging(builder =>
            {
                ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddSerilog(Log.Logger);
                });
            });
        })
         .ConfigureLogging(logging =>
         {
             logging.ClearProviders();
         })
        .UseSerilog((hostContext, services, logger) => {
            logger.ReadFrom.Configuration(configuration);
        })
        .Build();


    Log.Logger = logger;

    Log.Logger.Information("starting caterpillar control system...");

    return host;
}
