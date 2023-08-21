namespace StorageSpike.Host.Middleware.Headers;

public interface IHeaderService
{
    void SetHeader(string key, string[] value);

    string[] GetHeader(string key);

    IDictionary<string, string[]> GetAll();

    void AddHeaderToRequest(HttpRequestMessage httpRequestMessage, string key);

    void AddHeaderToRequest(HttpRequestMessage httpWebRequest, string key, string value);

    void AddHeaderToRequest(HttpRequestMessage httpWebRequest, string key, IEnumerable<string> values);
}
