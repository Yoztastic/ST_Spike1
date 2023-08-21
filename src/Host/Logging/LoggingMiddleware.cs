using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using StorageSpike.Host.Common.Logging;
using StorageSpike.Host.IoC;

namespace StorageSpike.Host.Logging;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;
    private readonly LoggingConfiguration _configuration;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger,
        IOptions<SerilogLoggingConfiguration> options)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task Invoke(HttpContext context)
    {
        var state = new Dictionary<string, object>();
        AddIfNotNull(ref state, CommonLoggingProperties.ConversationId, context.Request.Headers[DefaultHeaders.ConversationId]);
        AddIfNotNull(ref state, CommonLoggingProperties.ContextIdentity, context.Request.Headers[DefaultHeaders.ContextIdentity]);
        AddIfNotNull(ref state, CommonLoggingProperties.ClientComponent, context.Request.Headers[DefaultHeaders.UserAgent]);
        AddIfNotNull(ref state, CommonLoggingProperties.ClientIP, context.Connection.RemoteIpAddress?.ToString());

        if (_configuration.LogOriginalIp)
            AddIfNotNull(ref state, CommonLoggingProperties.OriginalIP, context.Request.Headers["X-Forwarded-For"]);

        using (_logger.BeginScope(state))
            await _next(context);
    }

    private static void AddIfNotNull(ref Dictionary<string, object> dic, string key, StringValues value)
    {
        if (!StringValues.IsNullOrEmpty(value)) dic.Add(key, value.ToString());
    }
}