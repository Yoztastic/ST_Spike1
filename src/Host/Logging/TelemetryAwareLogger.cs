using StorageSpike.Application.Core.Kernel;

namespace StorageSpike.Host.Logging;

public class TelemetryAwareLogger : ILogger
{
    private readonly ILogger _logger;
    private readonly ITelemetry _telemetry;

    public TelemetryAwareLogger(ILogger logger, ITelemetry telemetry)
    {
        _logger = logger;
        _telemetry = telemetry;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        if (state is not IDictionary<string, object?> dictionary) return _logger.BeginScope(state);
        _telemetry.Report(dictionary);
        return new NoopDisposable();
    }

    public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (_telemetry.Dictionary.Any())
        {
            using (_logger.BeginScope(_telemetry.Dictionary))
                _logger.Log(logLevel, eventId, state, exception, formatter);
        }
        else
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }

    private class NoopDisposable : IDisposable
    {
        public void Dispose()
        {
            // does nothing
        }
    }
}
