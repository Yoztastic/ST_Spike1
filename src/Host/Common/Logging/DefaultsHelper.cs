namespace StorageSpike.Host.Common.Logging;

public static class DefaultsHelper
{
    public static bool UseLogDebugLevel(IConfiguration configuration) =>
        string.Equals(configuration.GetValue<string>("Environment"), "Development",
            StringComparison.OrdinalIgnoreCase)
        || string.Equals(Environment.GetEnvironmentVariable(Constants.EnvironmentTypeEnvVariable)
                         ?? string.Empty, "Cluster", StringComparison.OrdinalIgnoreCase);
}