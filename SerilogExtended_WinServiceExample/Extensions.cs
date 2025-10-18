namespace SerilogExtended_WinServiceExample
{
    public static class Extensions
    {
        public static void ConfigureBackgroundOptions(this IServiceCollection services)
        {
            services.Configure<HostOptions>(options =>
            {
                options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
            });
        }

        public static void AddApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<Settings>()
                    .Bind(configuration.GetSection(nameof(Settings)))
                    .ValidateOnStart();
        }
    }
}