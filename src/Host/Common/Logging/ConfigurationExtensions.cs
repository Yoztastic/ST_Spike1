using Serilog;

namespace StorageSpike.Host.Common.Logging;

public static class ConfigurationExtensions
{
    private static readonly Assembly Assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

    public static LoggerConfiguration Configure(
        this LoggerConfiguration loggerConfiguration,
        IConfiguration configuration,
        LoggingConfiguration loggingConfiguration)
    {
        /*var useDebug = DefaultsHelper.UseLogDebugLevel(configuration);
        var configurationSection = configuration.GetSection("Uniper:Logging");
        var constantValues = BuildConstantValues(configurationSection);
        var jsonFormatter =
            new JsonFormatter(constantValues, loggingConfiguration.ExtraProperties, new OsInfoProvider());
        loggerConfiguration.WithConventions(configurationSection, GetFolder(configurationSection),
            loggingConfiguration.ConsoleAsJson, loggingConfiguration.WriteToFile, loggingConfiguration.WriteToSyslog, loggingConfiguration.WriteToConsole,
            useDebug,
            jsonFormatter);*/
        //TODO
        return loggerConfiguration;
    }


}
