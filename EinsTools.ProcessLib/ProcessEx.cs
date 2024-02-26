using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace EinsTools.ProcessLib;

public class ProcessEx(
    Process process, 
    SemaphoreSlim? semaOutput, 
    SemaphoreSlim? semaError,
    CancellationToken stoppingToken,
    ILogger? logger = null)
{
    public async Task<int> WaitForExitAsync()
    {
        if (logger is not null && logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Waiting for process {Process} to exit", 
                process.AsString());
        }
        await process.WaitForExitAsync(stoppingToken);
        if (semaOutput is not null)
        {
            await semaOutput.WaitAsync(stoppingToken);
        }
        if (semaError is not null)
        {
            await semaError.WaitAsync(stoppingToken);
        }
        if (logger is not null && logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Process {Process} has exited with code {ExitCode}", 
                process.AsString(), process.ExitCode);
        }
        return process.ExitCode;
    }
    
    public bool HasExited => process.HasExited;
}