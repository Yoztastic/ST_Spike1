using System.Text.Json;

namespace StorageSpike.Host.Common.Logging;

public class HttpRequestTrace
{
    public string Path { get; set; }
    public string QueryString { get; set; }
    public string Method { get; set; }
    public JsonDocument Body { get; set; }
    public Dictionary<string, string> Headers { get; set; }
}