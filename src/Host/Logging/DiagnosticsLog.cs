namespace StorageSpike.Host.Logging;

internal static class DiagnosticsLog
{
    private const string Empty = "-";

    public static void RequestStarting(this ILogger logger, HttpContext context)
    {
        var request = context.Request;
        logger.LogDebug(
            new EventId(1, "RequestStarting"),
            "Request starting {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} {ContentType} {ContentLength}",
            request.Protocol,
            request.Method,
            request.Scheme,
            request.Host.Value,
            request.PathBase.Value,
            request.Path.Value,
            request.QueryString.Value,
            FormatContentType(request.ContentType),
            request.ContentLength ?? 0);
    }

    public static void RequestFinished(this ILogger logger, HttpContext context, TimeSpan elapsed)
    {
        var response = context.Response;
        logger.LogDebug(
            new EventId(2, "RequestFinished"),
            "Request finished in {Duration}ms {StatusCode} {ContentType}",
            elapsed.TotalMilliseconds,
            response.StatusCode,
            response.ContentType);
    }

    public static void RequestFailedUnhandledException(this ILogger logger, Exception exception, TimeSpan elapsed)
    {
        logger.LogDebug(
            new EventId(3, "RequestFailedUnhandledException"),
            exception,
            "Request failed due to an unhandled exception in {Duration}ms",
            elapsed.TotalMilliseconds);
    }

    internal static string FormatContentType(string contentType)
        => contentType?.Length > 0 ? contentType.Replace(' ', '+') : Empty;
}