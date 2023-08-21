using Microsoft.Extensions.Logging;

namespace StorageSpike.Host.Common.Logging;

internal sealed class HttpContextTraceLogger : IHttpContextTracer
{
    private readonly ILogger<HttpContextTraceLogger> _logger;

    public HttpContextTraceLogger(ILogger<HttpContextTraceLogger> logger) => _logger = logger;

    public void Trace(HttpContextTrace trace) => _logger.Log(GetLevelByStatusCode(trace.Response.StatusCode), "HttpRequestResponse: {@trace}", trace);

    private static LogLevel GetLevelByStatusCode(int statusCode)
    {
        return statusCode switch
        {
            >= 500 => LogLevel.Error,
            >= 400 => LogLevel.Warning,
            _ => LogLevel.Information
        };
    }
}
