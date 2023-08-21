using System.Runtime.InteropServices;

namespace StorageSpike.Host.Common.Logging;

internal class OsInfoProvider : IOsInfoProvider
{
    public bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
}
