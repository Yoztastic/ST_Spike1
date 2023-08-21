using System.Runtime.InteropServices;
using Serilog;

namespace StorageSpike.Host.Common.Logging;

public sealed class SerilogLoggingConfiguration : LoggingConfiguration
{
    /// <summary>
    /// Custom action to apply any custom changes to the Serilog logger configuration
    /// </summary>
    public Action<LoggerConfiguration> LoggerConfiguration { get; set; }
}

public class LoggingConfiguration
{
    /// <summary>
    /// List of properties to be included from the logging scope and added to the extra section.
    /// </summary>
    public IList<string> ExtraProperties { get; set; } = new List<string>();

    /// <summary>
    /// Include all logger scope properties in the extras section. If set to true <see cref="ExtraProperties"/> will be ignored.
    /// </summary>
    public bool IncludeAllExtraProperties { get; set; } = false;

    /// <summary>
    /// Exclude these properties from the extras section, only when <see cref="IncludeAllExtraProperties"/> is set to true.
    /// </summary>
    public IList<string> ExcludeExtraProperties { get; set; } = new List<string>();

    /// <summary>
    /// Log to console output as Json. Defaults to false.
    /// </summary>
    public bool ConsoleAsJson { get; set; } = false;

    /// <summary>
    /// Write logs to file. Defaults to true on Windows. Ignored if the Folder is not set (either in configuration or through Conventional Deployments environment variables).
    /// </summary>
    public bool WriteToFile { get; set; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    /// <summary>
    /// If WriteToFile is enabled this controls the folder to write logs to.
    /// </summary>
    public string Folder { get; set; } = "Logs"; // should come from environment var

    /// <summary>
    /// Write logs to syslog. Defaults to true on Linux.
    /// </summary>
    public bool WriteToSyslog { get; set; } = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    /// <summary>
    /// Write logs to console. Defaults to true when running locally, false otherwise.
    /// </summary>
    public bool WriteToConsole { get; set; } = true; // could have some environment rules here

    /// <summary>
    /// Log the IP address in X-Forwarded-For to the originalip field. Defaults to false. Should be used for non public facing apps only.
    /// </summary>
    public bool LogOriginalIp { get; set; }

    /// <summary>
    /// All constant values to be included in log messages.
    /// </summary>
    public Dictionary<string, string> ConstantValues { get; set; } = new();
}
