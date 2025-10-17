using SerilogExtended.Implementation;

namespace SerilogExtended_WinServiceExample
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureBackgroundOptions();
            services.AddSerilogLogging(Configuration);
            services.AddApplicationSettings(Configuration);
            services.AddSingleton<IHostedService, BackgroundServiceSample>();
        }

        public void Configure(IApplicationBuilder app)
        {
            // nothing
        }
    }
}