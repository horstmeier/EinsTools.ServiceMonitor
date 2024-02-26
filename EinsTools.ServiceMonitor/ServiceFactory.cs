using EinsTools.ProcessLib;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;

namespace EinsTools.ServiceMonitor;

public class ServiceFactory(
    ILoggerFactory loggerFactory,
    IProcessHelper processHelper,
    IOptionsMonitor<RetryPolicy> defaultRetryPolicy) : IServiceFactory
{
    private readonly ILogger<ServiceFactory> _logger = loggerFactory.CreateLogger<ServiceFactory>();

    public IRunningService CreateService(string displayName, ServiceDescription serviceDescription,
        CancellationToken stoppingToken)
    {
        var cts = new CancellationTokenSource();
        var t = RunAsync(displayName, serviceDescription, cts.Token);
        return new RunningService(
            displayName,
            t,
            serviceDescription,
            cts,
            loggerFactory.CreateLogger<RunningService>(),
            stoppingToken);
    }

    private async Task RunAsync(string displayName, ServiceDescription serviceDescription,
        CancellationToken cancellationToken)
    {
        var retryCount = 0;
        var processLogger =
            serviceDescription.Logfile?.FileName == null
                ? new LoggerConfiguration().WriteTo.Console().CreateLogger()
                : new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.File(serviceDescription.Logfile.FileName,
                        fileSizeLimitBytes: serviceDescription.Logfile.MaxSizeInMB * 1024 * 1024,
                        retainedFileCountLimit: serviceDescription.Logfile.RetainedFileCountLimit)
                    .CreateLogger();
        while (!cancellationToken.IsCancellationRequested)
        {
            var retryPolicy = serviceDescription.RetryPolicy ?? defaultRetryPolicy.CurrentValue;

            // Delay before retrying
            if (retryCount > 0)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(retryPolicy.DelayInSeconds), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }

            ++retryCount;
            if (retryPolicy.MaxRetryCount > 0 && retryCount >= retryPolicy.MaxRetryCount)
            {
                _logger.LogInformation("Service {Service} has reached the maximum number of retries", displayName);
                return;
            }

            try
            {
                await RunProcessAsync(displayName, serviceDescription, cancellationToken, processLogger);
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                _logger.LogInformation(
                    "Service {Service} will be restarted in {Delay}s (Retry {RetryCount}/{MaxRetryCount})",
                    displayName, retryPolicy.DelayInSeconds, retryCount, retryPolicy.MaxRetryCount);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service {Service} has failed (Retry {RetryCount}/{MaxRetryCount})", displayName,
                    retryCount, retryPolicy.MaxRetryCount);
            }
        }
    }

    private async Task RunProcessAsync(string displayName, ServiceDescription serviceDescription,
        CancellationToken cancellationToken, Logger processLogger)
    {
        _logger.LogInformation("Starting service {Service}", displayName);
        processLogger.Information("Starting service {Service}", displayName);
        var exitCode = await processHelper.RunAsync(
            serviceDescription.FileName ?? throw new ArgumentNullException(nameof(serviceDescription.FileName)),
            serviceDescription.Arguments,
            serviceDescription.WorkingDirectory,
            serviceDescription.Environment,
            processLogger.Information,
            processLogger.Error,
            cancellationToken);
        _logger.LogInformation("Service {Service} has stopped with exit code {ExitCode}", displayName, exitCode);
        processLogger.Information("Service {Service} has stopped with exit code {ExitCode}", displayName, exitCode);
    }
}