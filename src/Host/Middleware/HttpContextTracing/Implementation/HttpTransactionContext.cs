using Infrastructure.Common;

namespace StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

internal class HttpTransactionContext : ITransactionContext
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpTransactionContext(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Dictionary<string, string> Headers
    {
        get
        {
            var requestHeaders = _contextAccessor.HttpContext?
                .Request.Headers.Select(kvp=>new KeyValuePair<string,string>(kvp.Key,kvp.Value));
            return new Dictionary<string, string>(requestHeaders ?? Enumerable.Empty<KeyValuePair<string,string>>() );
        }
    }
}
