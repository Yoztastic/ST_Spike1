namespace StorageSpike.Host.Middleware.Headers;

public static class HeadersExtensions
{
    public static string[] GetHeaderValues(this IHeaderDictionary headers, string headerName)
        => !headers.TryGetValue(headerName, out var stringValues)
            ? Array.Empty<string>()
            : stringValues.SelectMany(x => x.Split(',')).Select(x => x.Trim()).ToArray();

    public static IHeaderDictionary AddSecurityHeaders(this IHeaderDictionary headers)
    {
        foreach (var header in StandardHeaders.SecurityHeaders)
        {
            headers.AddOrReplace(header.Key, header.Value);
        }

        return headers;
    }

    public static IHeaderDictionary AddOrReplace(this IHeaderDictionary headers, string key, string[] value)
    {
        if (headers.ContainsKey(key))
        {
            headers.Remove(key);
        }

        headers.Add(key, new Microsoft.Extensions.Primitives.StringValues(value));

        return headers;
    }
}