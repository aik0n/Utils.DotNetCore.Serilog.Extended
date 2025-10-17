using Microsoft.Extensions.Options;

namespace SerilogExtended_WinServiceExample
{
    public class BackgroundServiceSample : BackgroundService
    {
        private readonly ILogger<BackgroundServiceSample> _logger;
        private readonly IOptions<Settings> _settings;

        public BackgroundServiceSample(ILogger<BackgroundServiceSample> logger, IOptions<Settings> settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                _logger.LogInformation("BackgroundServiceSample is running at: {time}", DateTimeOffset.Now);

                await Task.Delay(TimeSpan.FromSeconds(_settings.Value.LogMessagesIntervalSeconds), stoppingToken);
            }
        }
    }
}