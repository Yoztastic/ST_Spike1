namespace StorageSpike.Host.Common.Logging;

internal interface IOsInfoProvider
{
    bool IsWindows { get; }
}