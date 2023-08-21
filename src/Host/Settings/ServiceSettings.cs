using System.Diagnostics;

namespace StorageSpike.Host.Settings;

public sealed record ServiceSettings
{
    public string ApplicationId { get; init; } = "NOT DEFINED BY APPSETTINGS";
    public string ApplicationName { get; init; } = "NOT DEFINED BY APPSETTINGS";
    public string EnvironmentName { get; init; } = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "Dev";
    public string Slice { get; init; } = Environment.GetEnvironmentVariable("EM_SERVICE_SLICE") ?? "NA";

    public readonly Func<string> Version = () =>
    {
        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EM_SERVICE_VERSION"))) return Environment.GetEnvironmentVariable("EM_SERVICE_VERSION")!;

        return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion ?? "NOT DEFINED";
    };
}
