using StorageSpike.Host.Settings;

namespace StorageSpike.Host.Middleware;

internal static class HealthChecks
{
    // ReSharper disable once UnusedMethodReturnValue.Local - Fluent Style
    internal static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
    {
        // todo add any standard health check requirements here
        return app;
    }

    // example service info you may want on the health check
    private static Dictionary<string, object> GetServiceInformation(ServiceSettings settings) =>
        new()
        {
            { "ApplicationId", settings.ApplicationId },
            { "ServiceName", "StorageSpike" },
            { "Version", settings.Version() },
            { "Slice", settings.Slice }
        };
}
