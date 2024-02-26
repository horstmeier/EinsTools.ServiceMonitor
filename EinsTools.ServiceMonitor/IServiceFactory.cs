namespace EinsTools.ServiceMonitor;

public interface IServiceFactory
{
    public IRunningService CreateService(string displayName, ServiceDescription serviceDescription,
        CancellationToken stoppingToken);
}