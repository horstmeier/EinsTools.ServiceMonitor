namespace EinsTools.ServiceMonitor;

public interface IRunningService
{
    public Task StopAsync();
    public ServiceDescription ServiceDescription { get; }
}