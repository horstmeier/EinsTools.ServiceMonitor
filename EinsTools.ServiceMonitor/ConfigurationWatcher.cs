using System.Collections.Immutable;
using Microsoft.Extensions.Options;

namespace EinsTools.ServiceMonitor;

public interface IConfigurationWatcher
{
    Task RunServicesAsync(CancellationToken stoppingToken);
}

public class ConfigurationWatcher(
    IOptionsMonitor<Dictionary<string, ServiceDescription>> serviceDescriptionOptions,
    IServiceFactory serviceFactory) : IConfigurationWatcher
{
    public async Task RunServicesAsync(CancellationToken stoppingToken)
    {
        var semaChange = new SemaphoreSlim(0);
        serviceDescriptionOptions.OnChange(async (_, _) => { semaChange.Release(); });

        var runningServices = ImmutableDictionary<string, IRunningService>.Empty;

        while (!stoppingToken.IsCancellationRequested)
        {
            // Get the current service descriptions
            var serviceDescriptions = serviceDescriptionOptions.CurrentValue;
            // Stop services that are no longer in the configuration
            foreach (var (name, service) in runningServices)
            {
                if (!serviceDescriptions.TryGetValue(name, out var sd)
                    || !sd.Equals(service.ServiceDescription))
                {
                    await service.StopAsync();
                    runningServices = runningServices.Remove(name);
                }
            }
            
            // Now start all enabled services that are not running
            foreach (var (name, sd) in serviceDescriptions)
            {
                if (sd.Enabled && !name.StartsWith("-") && !runningServices.ContainsKey(name))
                {
                    var service = serviceFactory.CreateService(name, sd, stoppingToken);
                    runningServices = runningServices.Add(name, service);
                }
            }
;
            // Wait for a change in the configuration
            await semaChange.WaitAsync(stoppingToken);
        }
    }
}