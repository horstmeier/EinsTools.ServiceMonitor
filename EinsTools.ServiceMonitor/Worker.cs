namespace EinsTools.ServiceMonitor;

public class Worker(ILogger<Worker> logger, IConfigurationWatcher configurationWatcher)
    : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await configurationWatcher.RunServicesAsync(stoppingToken);
    }
}