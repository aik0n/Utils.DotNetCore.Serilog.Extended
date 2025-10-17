using Serilog;
using SerilogExtended.Implementation;
using Utils.DotNetCore.IServiceCollection;

namespace SerilogExtended_WinServiceExample
{
    internal class Program
    {
        private static string? _appName;
        private static string? _appGuid;

        private static IConfigurationRoot? _configuration;

        private static IConfiguration Configuration(string[] args)
        {
            if (null == _configuration)
            {
                _configuration = new ConfigurationBuilder()
                    .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                    .AddJsonFile("connections.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build();
            }

            return _configuration;
        }

        public static void Main(string[] args)
        {
            var logDiagnosticsPath = Configuration(args).GetConfigurationValue<string>(nameof(Settings), nameof(Settings.SerilogDebugPath));
            LoggerDebug.Enable(logDiagnosticsPath);

            _appName = "Sample.Windows.Service";
            _appGuid = Configuration(args).GetConfigurationValue<string>(nameof(Settings), nameof(Settings.ApplicationInstanceGuid));

            // only one application instance is allowed
            var mutex = new Mutex(false, "Global\\" + _appGuid, out bool createdNew);
            GC.KeepAlive(mutex);

            if (false == createdNew)
            {
                Log.Error("Instance already running");

                return;
            }

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Application {_appName}:{_appGuid} terminated unexpectedly");
                Log.Fatal(ex, $"Message: [{ex.Message}]");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(webBuilder =>
                {
                    webBuilder.AddJsonFile("serilog.json", optional: false, reloadOnChange: true);
                })
                .UseWindowsService(options =>
                {
                    options.ServiceName = _appName;
                })
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.ConfigureKestrel(options =>
                    {
                        var port = Configuration(args).GetConfigurationValue<int>(nameof(Settings), nameof(Settings.HostPort));
                        options.ListenAnyIP(port);
                    });
                    builder.UseConfiguration(Configuration(args));
                    builder.UseStartup<Startup>();
                });
    }
}