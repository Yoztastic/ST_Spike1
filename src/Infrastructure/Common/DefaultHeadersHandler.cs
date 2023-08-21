using Microsoft.Extensions.Options;

namespace Infrastructure.Common;

public class DefaultHeadersHandler : DelegatingHandler
{
    private readonly IContextAccessor _context;
    private readonly InfrastructureServiceSettings _serviceSettings;

    public DefaultHeadersHandler(IContextAccessor context, IOptions<InfrastructureServiceSettings> serviceSettings)
    {
        _context = context;
        _serviceSettings = serviceSettings.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.SafeAdd(HeaderNames.UserAgent, _serviceSettings.UserAgent);
        request.Headers.SafeAdd(HeaderNames.ConversationId, _context.ConversationId);
        request.Headers.SafeAdd(HeaderNames.ContextIdentity, _context.ContextIdentity);

        return await base.SendAsync(request, cancellationToken);
    }
}
