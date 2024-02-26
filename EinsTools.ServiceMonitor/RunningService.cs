using EinsTools.ProcessLib;

namespace EinsTools.ServiceMonitor;

public class RunningService(string displayName, 
    Task process, 
    ServiceDescription serviceDescription,
    CancellationTokenSource cts, 
    ILogger<RunningService> logger, 
    CancellationToken stoppingToken) 
    : IRunningService
{
    public async Task StopAsync()
    {
        if (process.IsCompleted)
        {
            logger.LogInformation("Service {Service} has already stopped", displayName);
            return;
        }
        logger.LogInformation("Stopping service {Service}", displayName);
        await cts.CancelAsync();
        await process.WaitAsync(stoppingToken);
        logger.LogInformation("Service {Service} has stopped", displayName);
    }

    public ServiceDescription ServiceDescription => serviceDescription;
}