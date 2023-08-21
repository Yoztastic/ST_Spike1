using JetBrains.Annotations;

namespace StorageSpike.Acceptance.Helpers.TestContracts.Diagnostics
{
    [PublicAPI]
    public class ServiceInformationContract
    {
        public string? ApplicationId { get; set; }
        public string? ServiceName { get; set; }
        public string? Version { get; set; }
        public string? Slice { get; set; }
        public bool IsHealthy { get; set; }
    }
}
