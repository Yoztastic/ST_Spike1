using System.Collections.Concurrent;
using StorageSpike.Host.IoC;

namespace StorageSpike.Host.Middleware.Headers;

public class SingletonHeaderService : IHeaderService
{
    private static readonly AsyncLocal<ConcurrentDictionary<string, string[]>> Headers = new();

    public void SetHeader(string key, string[] values)
    {
        Headers.Value ??= new ConcurrentDictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        if (key != null)
            Headers.Value[key] = values;
    }

    public string[] GetHeader(string key)
    {
        Headers.Value ??= new ConcurrentDictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        if (key != null && Headers.Value.TryGetValue(key, out var header))
            return header;

        return Array.Empty<string>();
    }

    public IDictionary<string, string[]> GetAll()
    {
        return Headers.Value ??
               (Headers.Value = new ConcurrentDictionary<string, string[]>(StringComparer.OrdinalIgnoreCase));
    }

    public void AddAllStandardHeadersToRequest(HttpRequestMessage httpRequestMessage)
    {
        foreach (var header in DefaultHeaders.All)
        {
            AddHeaderToRequest(httpRequestMessage, header);
        }
    }

    public void AddHeaderToRequest(HttpRequestMessage httpWebRequest, string key)
    {
        AddHeaderToRequest(httpWebRequest, key, GetHeader(key));
    }

    public void AddHeaderToRequest(HttpRequestMessage httpWebRequest, string key, string value)
    {
        AddHeaderToRequest(httpWebRequest, key, new[] { value });
    }

    public void AddHeaderToRequest(HttpRequestMessage httpWebRequest, string key, IEnumerable<string> values)
    {
        if (httpWebRequest.Headers.TryGetValues(key, out IEnumerable<string> _))
        {
            httpWebRequest.Headers.Remove(key);
        }

        var nonEmptyValues = values.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        if (nonEmptyValues.Any())
        {
            httpWebRequest.Headers.Add(key, nonEmptyValues);
        }
    }
}
