namespace StorageSpike.Application.Core.Kernel;

public static class ErrorCodes
{
    public const string UpstreamServiceFailure = nameof(UpstreamServiceFailure);
    public const string InternalServerError = nameof(InternalServerError);
    public const string InvalidIdFormat = nameof(InvalidIdFormat);
    public const string InvalidOperation = nameof(InvalidOperation);
    public const string NotFound = nameof(NotFound);
}
