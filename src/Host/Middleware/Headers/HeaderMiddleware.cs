namespace StorageSpike.Host.Middleware.Headers;

public class HeaderMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IEnumerable<string> _passthroughHeaders;

    public HeaderMiddleware(RequestDelegate next, IEnumerable<string>? passthroughHeaders = null)
    {
        _next = next;
        _passthroughHeaders = passthroughHeaders ?? Array.Empty<string>();
    }

    public async Task Invoke(HttpContext context, IHeaderService headerService)
    {
        var headerCache = new Dictionary<string, string?[]>();

        foreach (var requestHeader in context.Request.Headers)
        {
            headerCache.Add(requestHeader.Key, requestHeader.Value.ToArray());
            headerService.SetHeader(requestHeader.Key, requestHeader.Value.ToArray());
        }

        context.Response.OnStarting(() =>
        {
            foreach (var passthroughHeader in _passthroughHeaders)
            {
                if (context.Response.Headers.ContainsKey(passthroughHeader))
                    continue;

                if (headerCache.TryGetValue(passthroughHeader, out var values))
                {
                    var nonEmptyValues = values.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                    if (nonEmptyValues.Any())
                    {
                        context.Response.Headers.Add(passthroughHeader, nonEmptyValues);
                    }
                }

                context.Response.Headers.AddSecurityHeaders();
            }

            return Task.CompletedTask;
        });

        await _next(context);
    }
}