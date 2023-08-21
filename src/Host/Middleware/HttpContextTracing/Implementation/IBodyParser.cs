using System.Text.Json;

namespace StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

internal interface IBodyParser
{
    internal bool CanParse(string contentType);

    internal JsonDocument Parse(MemoryStream stream);
}
