using Microsoft.Extensions.Configuration;

namespace StorageSpike.Host.Tests.EndToEnd.Acceptance;

public static class ConfigurationService
{
    private const string _development = "Development";

    static ConfigurationService() => Env = Environment.GetEnvironmentVariable("DEPLOYMENT_ENV_NAME") ?? _development;

    public static string Env { get;}

    public static AppSettings Load()
    {
        var configurationRoot = LoadConfigurationRoot();

        return configurationRoot.Get<AppSettings>();
    }

    private static IConfigurationRoot LoadConfigurationRoot()
    {
        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder
            .AddJsonFile("./appsettings.e2e.json", false);

        if (Env != _development)
            configurationBuilder.AddJsonFile("./appsettings.environment.json", true);

        return configurationBuilder.AddEnvironmentVariables().Build();
    }
}
