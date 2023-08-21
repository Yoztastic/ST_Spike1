using System.Diagnostics;

namespace StorageSpike.Host.Logging;

public class DiagnosticsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DiagnosticsMiddleware> _logger;
    private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

    public DiagnosticsMiddleware(RequestDelegate next, ILogger<DiagnosticsMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // perf: avoid async state machine when not in debug
    public Task Invoke(HttpContext context) =>
        _logger.IsEnabled(LogLevel.Debug)
            ? InvokeInternalAsync(context)
            : _next(context);

    private async Task InvokeInternalAsync(HttpContext context)
    {
        var startTimestamp = Stopwatch.GetTimestamp();
        _logger.RequestStarting(context);

        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.RequestFailedUnhandledException(exception, CalculateElapsed(startTimestamp));
            throw;
        }

        _logger.RequestFinished(context, CalculateElapsed(startTimestamp));
    }

    private static TimeSpan CalculateElapsed(long startTimestamp)
    {
        var currentTimestamp = Stopwatch.GetTimestamp();
        var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));
        return elapsed;
    }
}