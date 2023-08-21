using Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using StorageSpike.Application.Core;
using StorageSpike.Application.Core.Kernel;
using StorageSpike.Host.Settings;

namespace StorageSpike.Host.Middleware;

public class TelemetryReportingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITelemetryReporter _telemetryReporter;
    private readonly ITelemetry _telemetry;
    private readonly string _version;
    private readonly string _slice;

    public TelemetryReportingMiddleware(RequestDelegate next, IOptions<ServiceSettings> options, ITelemetryReporter telemetryReporter, ITelemetry telemetry)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _telemetryReporter = telemetryReporter;
        _telemetry = telemetry;
        _version = options.Value.Version();
        _slice = options.Value.Slice;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _telemetryReporter.Report(ReportingConstants.AppVersion,_version);
            _telemetryReporter.Report(ReportingConstants.Slice,_slice);
            await _next(context);
            _telemetryReporter.Report(_telemetry.Dictionary);
        }
        catch (Exception e)
        {
            _telemetry.Report(ReportingConstants.ErrorType, e.GetType().Name);
            _telemetry.Report(ReportingConstants.ErrorMessage, e.Message);
            _telemetryReporter.Report(_telemetry.Dictionary);
            throw;
        }
    }
}
