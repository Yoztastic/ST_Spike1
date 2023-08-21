namespace StorageSpike.Application.Core;

public static class ReportingConstants
{
    static ReportingConstants() => All = typeof(ReportingConstants).GetAllStringFields();
    public static IList<string> All { get; }

    // Add important attributes for telemetry here
    public const string ErrorType = nameof(ErrorType);
    public const string ErrorMessage = nameof(ErrorMessage);
    public const string AppVersion = nameof(AppVersion);
    public const string Slice = nameof(Slice);
}
