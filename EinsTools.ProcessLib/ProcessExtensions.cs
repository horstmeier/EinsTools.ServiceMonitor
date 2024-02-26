using System.Diagnostics;

namespace EinsTools.ProcessLib;

public static class ProcessExtensions
{
    public static string AsString(this ProcessStartInfo process) =>
        process.ArgumentList.Count == 0 
            ? $"{process.FileName}({string.Join(',', process.ArgumentList)})" 
            : $"{process.FileName}({process.Arguments})";
    
    public static string AsString(this Process process) => process.StartInfo.AsString();
}