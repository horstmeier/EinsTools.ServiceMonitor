using System.Diagnostics;

namespace EinsTools.ProcessLib;

public interface IProcessHelper
{
    /// <summary>
    /// Executes a process asynchronously.
    /// </summary>
    /// <param name="processStartInfo">The information about the process to start.</param>
    /// <param name="outputDataReceived">An action to perform when output data is received from the process.</param>
    /// <param name="errorDataReceived">An action to perform when error data is received from the process.</param>
    /// <param name="stoppingToken">A cancellation token that can be used to cancel the process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a ProcessEx object.</returns>
    Task<ProcessEx> ExecuteAsync(
        ProcessStartInfo processStartInfo,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null,
        CancellationToken stoppingToken = default);

    /// <summary>
    /// Runs a process asynchronously and returns the exit code.
    /// </summary>
    /// <param name="processStartInfo">The information about the process to start.</param>
    /// <param name="outputDataReceived">An action to perform when output data is received from the process.</param>
    /// <param name="errorDataReceived">An action to perform when error data is received from the process.</param>
    /// <param name="stoppingToken">A cancellation token that can be used to cancel the process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the exit code of the process.</returns>

    async Task<int> RunAsync(
        ProcessStartInfo processStartInfo,
        Action<string>? outputDataReceived,
        Action<string>? errorDataReceived,
        CancellationToken stoppingToken = default)
    {
        var p = await ExecuteAsync(processStartInfo, outputDataReceived, 
            errorDataReceived, stoppingToken);
        return await p.WaitForExitAsync();
    }
}