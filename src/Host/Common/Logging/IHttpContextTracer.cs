namespace StorageSpike.Host.Common.Logging;

public interface IHttpContextTracer
{
    void Trace(HttpContextTrace trace);
}