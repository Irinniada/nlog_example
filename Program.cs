using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.LayoutRenderers;
using nlogEx.IOC;

const string NLogMainFileNameVariable = "mainLogFileName";
const string NLogErrorFileNameVariable = "errorLogFileName";

var host = Host.CreateDefaultBuilder(args);
host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddCommandLine(args)
    .AddEnvironmentVariables()
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();

ConfigLogging(config);

// Register services directly with Autofac here. Don't
// call builder.Populate(), that happens in AutofacServiceProviderFactory.
host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new WorkingModule());
    containerBuilder.RegisterType<NLogLoggerFactory>().As<ILoggerFactory>().SingleInstance();
});  

var app = host.Build();

var logger = LogManager.Setup().ReloadConfiguration().GetCurrentClassLogger();
logger.Info("--------------------------------------");
logger.Info("Service is started");


await app.RunAsync();

void ConfigLogging(IConfiguration config)
{
    LayoutRenderer.Register("serviceName", (logEvent) => "Example");

    ConfigSettingLayoutRenderer.DefaultConfiguration = config;
    LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));
    
    var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Environment.CurrentDirectory;
    SetErrorLoggerTargetFileName(assemblyFolder);
    SetLoggerTargetFileNames(assemblyFolder);

    LogManager.ReconfigExistingLoggers();
}

void SetLoggerTargetFileNames(string rootPath)
{
    GlobalDiagnosticsContext.Set(NLogMainFileNameVariable,
        Path.Combine(
            rootPath, 
            "logs", 
            $"{DateTime.Now:yyyy-MM-dd}",
            $"log_{DateTime.Now:yyyy-MM-dd-HH}.log"));
}

void SetErrorLoggerTargetFileName(string rootPath)
{
    GlobalDiagnosticsContext.Set(NLogErrorFileNameVariable,
        Path.Combine(
            rootPath, 
            "logs", 
            $"{DateTime.Now:yyyy-MM-dd}", 
            "errors.log"));
}
