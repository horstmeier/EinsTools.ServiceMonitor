using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace EinsTools.ProcessLib;

public class ProcessHelper(ILogger<ProcessHelper>? logger = null) : IProcessHelper
{
    public Task<ProcessEx> ExecuteAsync(
        ProcessStartInfo processStartInfo, 
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null, 
        CancellationToken stoppingToken = default)
    {
        if (logger != null && logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Starting process {Process}", 
                processStartInfo.AsString());
        }
        
        var process = new Process
        {
            StartInfo = processStartInfo,
            EnableRaisingEvents = true
        };
        
        SemaphoreSlim? semaOutput = null;
        if (outputDataReceived is not null)
        {
            process.StartInfo.RedirectStandardOutput = true;
            semaOutput = new SemaphoreSlim(0);
            process.OutputDataReceived += (_, args) =>
            {
                if (args.Data is not null)
                {
                    outputDataReceived(args.Data);
                }
                else
                {
                    semaOutput.Release();
                }
            };
        }
        SemaphoreSlim? semaError = null;
        if (errorDataReceived is not null)
        {
            process.StartInfo.RedirectStandardError = true;
            semaError = new SemaphoreSlim(0);
            process.ErrorDataReceived += (_, args) =>
            {
                if (args.Data is not null)
                {
                    errorDataReceived(args.Data);
                }
                else
                {
                    semaError.Release();
                }
            };
        }
        process.Start();
        if (semaOutput is not null)
        {
            process.BeginOutputReadLine();
        }
        if (semaError is not null)
        {
            process.BeginErrorReadLine();
        }
        return Task.FromResult(new ProcessEx(process, semaOutput, semaError, stoppingToken, logger));
    }
    
    
}