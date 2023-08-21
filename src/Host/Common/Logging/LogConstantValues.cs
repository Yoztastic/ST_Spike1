namespace StorageSpike.Host.Common.Logging;

public readonly struct LogConstantValues
{
    /// <summary>
    /// Constant values for log entries.
    /// </summary>
    /// <param name="appName">Service name as defined in Environment Manager. It will be logged in the 'component' field.</param>
    /// <param name="appVersion">Version of the service, in SemVer 2.0.0 format.</param>
    /// <param name="envShortName">The short-name of the environment. It will be logged in the 'environment' field.</param>
    /// <param name="hostName">The hostname, usually retrieved from <see cref="System.Environment.MachineName"/> (requires .Net Standard 1.5). It will be logged in the 'environment' field.</param>
    /// <param name="slice">Slice name as blue or green. Only for blue-green deployments.</param>
    public LogConstantValues(string appName, string appVersion, string envShortName,
        string hostName, string slice = null)
    {
        AppName = appName?.ToLowerInvariant();
        AppVersion = appVersion?.ToLowerInvariant();
        EnvShortName = envShortName?.ToLowerInvariant();
        HostName = hostName?.ToLowerInvariant();
        Slice = string.IsNullOrWhiteSpace(slice) ? null : slice;
    }

    public string AppName { get; }
    public string AppVersion { get; }
    public string EnvShortName { get; }
    public static string EventType => "ApplicationLogs";
    public string HostName { get; }
    public string Slice { get; }
}