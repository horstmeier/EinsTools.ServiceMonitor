using System.Diagnostics;
using System.Text;

namespace EinsTools.ProcessLib;

public static class ProcessHelperExtensions
{
    /// <summary>
    /// Executes a process asynchronously using the provided file name and optional parameters.
    /// </summary>
    /// <param name="processHelper">The IProcessHelper instance this method extends.</param>
    /// <param name="fileName">The name of the file to execute.</param>
    /// <param name="arguments">Optional arguments for the process.</param>
    /// <param name="workingDirectory">Optional working directory for the process.</param>
    /// <param name="environmentVariables">Optional environment variables for the process.</param>
    /// <param name="outputDataReceived">An action to perform when output data is received from the process.</param>
    /// <param name="errorDataReceived">An action to perform when error data is received from the process.</param>
    /// <param name="stoppingToken">A cancellation token that can be used to cancel the process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a ProcessEx object.</returns>
    public static async Task<ProcessEx> ExecuteAsync(
        this IProcessHelper processHelper,
        string fileName,
        string? arguments = null,
        string? workingDirectory = null,
        IDictionary<string, string?>? environmentVariables = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null,
        CancellationToken stoppingToken = default)
    {
        var psi = BuildProcessStartInfo(fileName, arguments, null, workingDirectory, 
            environmentVariables);
        return await processHelper.ExecuteAsync(psi, outputDataReceived, 
            errorDataReceived, stoppingToken);
    }
    
    /// <summary>
    /// Executes a process asynchronously using the provided file name and optional parameters.
    /// </summary>
    /// <param name="processHelper">The IProcessHelper instance this method extends.</param>
    /// <param name="fileName">The name of the file to execute.</param>
    /// <param name="arguments">Optional arguments for the process.</param>
    /// <param name="workingDirectory">Optional working directory for the process.</param>
    /// <param name="environmentVariables">Optional environment variables for the process.</param>
    /// <param name="outputDataReceived">An action to perform when output data is received from the process.</param>
    /// <param name="errorDataReceived">An action to perform when error data is received from the process.</param>
    /// <param name="stoppingToken">A cancellation token that can be used to cancel the process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a ProcessEx object.</returns>
    public static async Task<ProcessEx> ExecuteAsync(
        this IProcessHelper processHelper,
        string fileName,
        string[] arguments,
        string? workingDirectory = null,
        IDictionary<string, string?>? environmentVariables = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null,
        CancellationToken stoppingToken = default)
    {
        var psi = BuildProcessStartInfo(fileName, null, arguments, workingDirectory, 
            environmentVariables);
        return await processHelper.ExecuteAsync(psi, outputDataReceived, 
            errorDataReceived, stoppingToken);
    }
    
    public static async Task<ProcessResult> RunAndGetOutputAsync(
        this IProcessHelper processHelper,
        ProcessStartInfo processStartInfo,
        CancellationToken stoppingToken = default)
    {
        var sbOut = new StringBuilder();
        var sbErr = new StringBuilder();
        var p = await processHelper.ExecuteAsync(processStartInfo, 
            s => sbOut.AppendLine(s), 
            s => sbErr.AppendLine(s), stoppingToken);
        var exitCode = await p.WaitForExitAsync();
        return new ProcessResult(exitCode, sbOut.ToString(), sbErr.ToString());
    }
    
    /// <summary>
    /// Executes a process asynchronously using the provided file name and optional parameters, and returns the exit code of the process.
    /// </summary>
    /// <param name="processHelper">The IProcessHelper instance this method extends.</param>
    /// <param name="fileName">The name of the file to execute.</param>
    /// <param name="arguments">Optional arguments for the process.</param>
    /// <param name="workingDirectory">Optional working directory for the process.</param>
    /// <param name="environmentVariables">Optional environment variables for the process.</param>
    /// <param name="outputDataReceived">An action to perform when output data is received from the process.</param>
    /// <param name="errorDataReceived">An action to perform when error data is received from the process.</param>
    /// <param name="stoppingToken">A cancellation token that can be used to cancel the process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the exit code of the process.</returns>
    public static async Task<int> RunAsync(
        this IProcessHelper processHelper,
        string fileName,
        string? arguments = null,
        string? workingDirectory = null,
        IDictionary<string, string?>? environmentVariables = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null,
        CancellationToken stoppingToken = default)
    {
        var psi = BuildProcessStartInfo(fileName, arguments, null, workingDirectory, 
            environmentVariables);
        return await processHelper.RunAsync(psi, outputDataReceived, errorDataReceived, stoppingToken);
    }

    /// <summary>
    /// Executes a process asynchronously using the provided file name and an array of arguments, and returns the exit code of the process.
    /// </summary>
    /// <param name="processHelper">The IProcessHelper instance this method extends.</param>
    /// <param name="fileName">The name of the file to execute.</param>
    /// <param name="arguments">An array of arguments for the process.</param>
    /// <param name="workingDirectory">Optional working directory for the process.</param>
    /// <param name="environmentVariables">Optional environment variables for the process.</param>
    /// <param name="outputDataReceived">An action to perform when output data is received from the process.</param>
    /// <param name="errorDataReceived">An action to perform when error data is received from the process.</param>
    /// <param name="stoppingToken">A cancellation token that can be used to cancel the process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the exit code of the process.</returns>
    public static async Task<int> RunAsync(
        this IProcessHelper processHelper,
        string fileName,
        string[] arguments,
        string? workingDirectory = null,
        IDictionary<string, string?>? environmentVariables = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null,
        CancellationToken stoppingToken = default)
    {
        var psi = BuildProcessStartInfo(fileName, null, arguments, workingDirectory, 
            environmentVariables);
        return await processHelper.RunAsync(psi, outputDataReceived, errorDataReceived, stoppingToken);
    }
    
    /// <summary>
    /// Executes a process asynchronously using the provided file name and an array of arguments, and returns the process result.
    /// </summary>
    /// <param name="processHelper">The IProcessHelper instance this method extends.</param>
    /// <param name="fileName">The name of the file to execute.</param>
    /// <param name="arguments">An array of arguments for the process.</param>
    /// <param name="workingDirectory">Optional working directory for the process.</param>
    /// <param name="environmentVariables">Optional environment variables for the process.</param>
    /// <param name="stoppingToken">A cancellation token that can be used to cancel the process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the exit code, output, and error of the process.</returns>
    public static async Task<ProcessResult> RunAndGetOutputAsync(
        this IProcessHelper processHelper,
        string fileName,
        string? arguments = null,
        string? workingDirectory = null,
        IDictionary<string, string?>? environmentVariables = null,
        CancellationToken stoppingToken = default)
    {
        var psi = BuildProcessStartInfo(fileName, arguments, null, workingDirectory, 
            environmentVariables);
        return await RunAndGetOutputAsync(processHelper, psi, stoppingToken);
    }
    
    /// <summary>
    /// Executes a process asynchronously using the provided file name and an array of arguments, and returns the process result.
    /// </summary>
    /// <param name="processHelper">The IProcessHelper instance this method extends.</param>
    /// <param name="fileName">The name of the file to execute.</param>
    /// <param name="arguments">An array of arguments for the process.</param>
    /// <param name="workingDirectory">Optional working directory for the process.</param>
    /// <param name="environmentVariables">Optional environment variables for the process.</param>
    /// <param name="stoppingToken">A cancellation token that can be used to cancel the process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the exit code, output, and error of the process.</returns>
    public static async Task<ProcessResult> RunAndGetOutputAsync(
        this IProcessHelper processHelper,
        string fileName,
        string[] arguments,
        string? workingDirectory = null,
        IDictionary<string, string?>? environmentVariables = null,
        CancellationToken stoppingToken = default)
    {
        var psi = 
            BuildProcessStartInfo(fileName, null, arguments, workingDirectory, 
                environmentVariables);
        return await RunAndGetOutputAsync(processHelper, psi, stoppingToken);
    }
    
    private static ProcessStartInfo BuildProcessStartInfo(
        string fileName,
        string? arguments,
        string[]? argumentList,
        string? workingDirectory,
        IDictionary<string, string?>? environmentVariables)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            WorkingDirectory = workingDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        
        if (argumentList is not null)
        {
            foreach (var argument in argumentList)
            {
                psi.ArgumentList.Add(argument);
            }
        }
        else
        {
            psi.Arguments = arguments;
        }
        
        if (environmentVariables is not null)
        {
            foreach (var (key, value) in environmentVariables)
            {
                psi.EnvironmentVariables[key] = value;
            }
        }
        return psi;
    }
}