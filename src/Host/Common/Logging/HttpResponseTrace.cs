using System.Text.Json;

namespace StorageSpike.Host.Common.Logging;

public class HttpResponseTrace
{
    public int StatusCode { get; set; }
    public JsonDocument Body { get; set; }
    public Dictionary<string, string> Headers { get; set; }
}