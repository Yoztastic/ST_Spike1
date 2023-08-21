using StorageSpike.Application.Core.Kernel.Extensions;
using StorageSpike.Host.Exceptions;

namespace StorageSpike.Host.Middleware;

public class MandatoryHeaderMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IEnumerable<string> _mandatoryHeaders;

    public MandatoryHeaderMiddleware(RequestDelegate next, IEnumerable<string>? mandatoryHeaders = null)
    {
        _next = next;
        _mandatoryHeaders = mandatoryHeaders ?? Enumerable.Empty<string>();
    }

    public async Task Invoke(HttpContext context)
    {
        var missingHeaders = _mandatoryHeaders.Where(HeaderIsMissingFromRequest(context));
        if(missingHeaders.Any())
            throw new MissingMandatoryHeaderException(missingHeaders.Select(h=>$"missing.{h}").Join());
        await _next.Invoke(context);
    }

    private static Func<string,bool> HeaderIsMissingFromRequest(HttpContext context) =>
        header =>!context.Request.Headers.ContainsKey(header);
}
