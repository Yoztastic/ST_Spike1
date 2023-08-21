using StorageSpike.Application.Core.Kernel;

namespace StorageSpike.Host.Logging;

public class TelemetryAwareLoggerFactory : ILoggerFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ITelemetry _telemetry;

    public TelemetryAwareLoggerFactory(ILoggerFactory loggerFactory, ITelemetry telemetry)
    {
        _loggerFactory = loggerFactory;
        _telemetry = telemetry;
    }

    public void Dispose() => _loggerFactory.Dispose();

    public void AddProvider(ILoggerProvider provider) => _loggerFactory.AddProvider(provider);

    public ILogger CreateLogger(string categoryName) => new TelemetryAwareLogger(_loggerFactory.CreateLogger(categoryName), _telemetry);
}
