using System.Diagnostics;
using System.Text;
using EinsTools.ProcessLib;

using static System.Console;

var processHelper = new ProcessHelper();

var tmpDir = Path.GetTempPath();
Directory.Delete(tmpDir, true);
Directory.CreateDirectory(tmpDir);

var processStartInfo = new ProcessStartInfo("dotnet", "new console -n TestApp")
{
    WorkingDirectory = tmpDir
};

var output = new StringBuilder();
var error = new StringBuilder();

var process = await processHelper.ExecuteAsync(processStartInfo, 
    data => output.AppendLine(data), 
    data => error.AppendLine(data));

var exitCode = await process.WaitForExitAsync();

if (exitCode != 0)
{
    WriteLine("Error:");
    WriteLine(error.ToString());
    return 1;
}

var testAppDir = Path.Combine(tmpDir, "TestApp");

if (!File.Exists(Path.Combine(testAppDir, "Program.cs")))
{
    WriteLine("Error: Program.cs not found");
    return 1;
}

if (!File.Exists(Path.Combine(testAppDir, "TestApp.csproj")))
{
    WriteLine("Error: TestApp.csproj not found");
    return 1;
}

WriteLine("Output:");
WriteLine(output.ToString());
WriteLine("Error:");
WriteLine(error.ToString());
return 0;