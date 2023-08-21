namespace StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

internal static class DefaultWhitelistedHeaders
{
    internal static readonly string[] All = {
        "ContextIdentity",
        "ConversationId",
        "User-Agent",
        "Content-Type",
        "Accept",
        "Host",
        "Content-Length"
    };
}
