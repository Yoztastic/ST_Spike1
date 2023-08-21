namespace StorageSpike.Host.Common;

public class CircuitBreakerSettings
{
    public const int DefaultFailureThreshold = 10;
    public const int DefaultRetriesCount = 3;

    public int FailureThreshold { get; set; } = DefaultFailureThreshold;
    public TimeSpan BreakDuration { get; set; } = TimeSpan.FromMinutes(1);
    public int RetriesCount { get; set; } = DefaultRetriesCount;
    public static string SectionName => "CircuitBreaker";
}
