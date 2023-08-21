using System.Diagnostics;
using StorageSpike.Host.Common.Logging;
using StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

namespace StorageSpike.Host.Middleware.HttpContextTracing;

internal class HttpContextTracingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TracingConfiguration<HttpContext> _configuration;
    private readonly Action<HttpContextTrace>[] _tracingActions;

    public HttpContextTracingMiddleware(RequestDelegate next, TracingConfiguration<HttpContext> configuration, Action<HttpContextTrace>[] tracingActions)
    {
        _next = next;
        _configuration = configuration;
        _tracingActions = tracingActions;
    }

    public async Task Invoke(HttpContext context)
    {
        var excludeTracing = _configuration.Exclusions.Any(condition => condition(context));

        if (excludeTracing)
        {
            await _next.Invoke(context);
        }
        else
        {
            try
            {
                await InvokeAndTraceAsync(context);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"An error has occurred tracing HttpContext: {ex.Message}");
            }
        }
    }

    private async Task InvokeAndTraceAsync(HttpContext context)
    {
        using var tracingWrapper = new HttpContextTracingWrapper(_configuration, context, _tracingActions);

        await tracingWrapper.BeginAsync();

        await _next.Invoke(context);

        await tracingWrapper.EndAsync();
    }
}
