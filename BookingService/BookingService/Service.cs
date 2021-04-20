using BookingService.Configuration;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NpgsqlTypes;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BookingService
{
    public class Service
    {
        /// <summary>
        /// Gets or sets the service's configuration properties. 
        /// </summary>
        /// <remarks>
        /// Please do not use this property directly; instead use dependency injection to get access to the service's configuration.
        /// </remarks>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Used to set the applications start time.
        /// </summary>
        private static DateTime? _startTime = null;

        /// <summary>
        /// Get's the uptime of the web host.
        /// </summary>
        internal static TimeSpan Uptime => (Service._startTime.HasValue) ? DateTime.UtcNow - Service._startTime.Value : new TimeSpan(0, 0, 0);

        /// <summary>
        /// Gets and sets the log level dynamically
        /// </summary>
        public static LoggingLevelSwitch LoggingLevelSwitch { get; set; }

        /// <summary>
        /// Gets and sets the microsoft log level dynamically
        /// </summary>
        public static LoggingLevelSwitch MicrosoftLoggingLevelSwitch { get; set; }


        public static void Main(string[] args)
        {
            Service._startTime = DateTime.UtcNow;

            // Create and configure the web service
            var webHost = CreateWebHostBuilder(args).Build();

            try
            {
                // Get access to required configuration service
                var configuration = webHost.Services.GetRequiredService<IConfiguration>();


                LoggingLevelSwitch = new LoggingLevelSwitch { MinimumLevel = LogEventLevel.Debug };
                MicrosoftLoggingLevelSwitch = new LoggingLevelSwitch { MinimumLevel = LogEventLevel.Error };

                //configure Serilog Postgres sink.  This is all default from the docs.

                IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
                {
                    {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                    {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                    {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                    {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                    {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                    {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                    {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                    {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
                };

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                    .MinimumLevel.ControlledBy(LoggingLevelSwitch)
                    .MinimumLevel.Override("Microsoft", MicrosoftLoggingLevelSwitch)
#if DEBUG
                    .WriteTo.Debug()
                    .WriteTo.Console()
#endif
                    //.WriteTo.PostgreSQL(
                    //    connectionString: configuration.GetConnectionString("BookingService"),
                    //    tableName: "Logs",
                    //    needAutoCreateTable: true,
                    //    columnOptions: columnWriters
                    //    )
                    .WriteTo.File(
                        $"{configuration.GetLogFilePath()}{$"{"BookingService"}_{DateTime.UtcNow:yyyy_MM_dd}"}.log",
                        retainedFileCountLimit: 10,
                        rollingInterval: RollingInterval.Day)
                    .CreateLogger();

                #region Serilog local debug
#if DEBUG
                Serilog.Debugging.SelfLog.Enable(msg =>
                {
                    Debug.Print(msg);
                    Debugger.Break();
                });
#endif
                #endregion

                //get the logger after Serilog configuration above
                var logger = webHost.Services.GetRequiredService<ILogger<Service>>();
                logger?.LogInformation($"The {configuration.GetServiceName()} service is starting (Process Name: {Process.GetCurrentProcess().ProcessName}, Process ID: {Process.GetCurrentProcess().Id}).");

                logger?.LogInformation($"The {configuration.GetServiceName()} service has started");

                // Run the web service
                webHost.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }


        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)

                .ConfigureAppConfiguration((context, config) =>
                {
                    // Add support for JSON settings from a JSON file
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("appsettings.{env.EnvironmentName}.json", optional: true);

                    // Add support for environment variables that override JSON settings
                    config.AddEnvironmentVariables(prefix: "LODS_");

                    // Add support for command line variables that override JSON and environment variable settings
                    config.AddCommandLine(args);
                })
                // Log!  It's better than bad, it's good!  You're gonna love a log!
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
