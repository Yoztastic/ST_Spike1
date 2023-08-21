namespace StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

internal static class DefaultMaskedFields
{
    private const string Authorization = "Authorization";

    internal static readonly string[] All = {Authorization};
}
