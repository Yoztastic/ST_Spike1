using StorageSpike.Application.Core.Kernel;
using StorageSpike.Host.IoC;

namespace StorageSpike.Host.Middleware.Headers;

public class HeaderTelemetryMiddleware
{
    private readonly RequestDelegate _next;

    public HeaderTelemetryMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context, ITelemetry telemetry)
    {
        DefaultHeaders.All.ToList().ForEach(ReportHeader(context, telemetry));
        await _next.Invoke(context);
    }

    private static Action<string> ReportHeader(HttpContext context, ITelemetry telemetry) =>
        headerName=>telemetry.Report(headerName,context.Request.Headers.TryGetValue(headerName, out var value)?value:"missing");
}
