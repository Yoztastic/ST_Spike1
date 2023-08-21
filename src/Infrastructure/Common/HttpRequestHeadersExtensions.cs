using System.Net.Http.Headers;

namespace Infrastructure.Common;

public static class HttpRequestHeadersExtensions
{
    public static void SafeAdd(this HttpRequestHeaders headers, string key, string? value)
    {
        if (value != null && !headers.Contains(key)) headers.Add(key, value);
    }
}
