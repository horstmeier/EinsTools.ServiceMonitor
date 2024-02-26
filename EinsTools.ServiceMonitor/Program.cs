using EinsTools.ProcessLib;
using EinsTools.ServiceMonitor;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);
// Register the default retry policy as IOptions
builder.Services.Configure<RetryPolicy>(builder.Configuration.GetSection("RetryPolicy"));
// Register the service descriptions as IOptions
builder.Services.Configure<Dictionary<string, ServiceDescription>>(builder.Configuration.GetSection("Services"));
// Register the IPorcessHelper
builder.Services.AddSingleton<IProcessHelper, ProcessHelper>(c=>
    new ProcessHelper(c.GetRequiredService<ILogger<ProcessHelper>>()));
// Register the service factory
builder.Services.AddSingleton<IServiceFactory, ServiceFactory>(c =>
    new ServiceFactory(c.GetRequiredService<ILoggerFactory>(),
        c.GetRequiredService<IProcessHelper>(),
        c.GetRequiredService<IOptionsMonitor<RetryPolicy>>()));
// Register the configuration manager
builder.Services.AddSingleton<IConfigurationWatcher>(c=>
    new ConfigurationWatcher(
        c.GetRequiredService<IOptionsMonitor<Dictionary<string, ServiceDescription>>>(),
        c.GetRequiredService<IServiceFactory>()));
// Register the worker
builder.Services.AddHostedService<Worker>(c =>
    new Worker( c.GetRequiredService<ILogger<Worker>>(), 
        c.GetRequiredService<IConfigurationWatcher>()));

#if WINDOWS
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "EinsTools ServiceMonitor";
});
#endif

var host = builder.Build();
host.Run();