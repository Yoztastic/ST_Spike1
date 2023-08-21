namespace StorageSpike.Host.Common.Logging;

public  class HttpContextTrace
{
    public HttpRequestTrace Request { get; set; }
    public HttpResponseTrace Response { get; set; }
    public string Identity { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime DateTimeUtc { get; set; }
    public IDictionary<string, object> Environment { get; set; }
}
