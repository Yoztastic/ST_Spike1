using System.Text.Json;

namespace StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

internal class JsonBodyParser : IBodyParser
{
    private const string ApplicationJsonMediaType = "application/json";
    private const string JsonMediaTypeSuffix = "+json";

    public bool CanParse(string contentType)
    {
        var mediaTypes = ContentTypeHelper.GetMediaTypes(contentType);
        var canParse = mediaTypes.Any(mediaType => IsJsonMediaType(mediaType) || HasJsonSuffix(mediaType));

        return canParse;
    }

    public JsonDocument Parse(MemoryStream stream) => JsonDocument.Parse(stream);

    private static bool IsJsonMediaType(string mediaType)
    {
        var isJsonMediaType = string.Equals(mediaType, ApplicationJsonMediaType, StringComparison.InvariantCultureIgnoreCase);

        return isJsonMediaType;
    }

    private static bool HasJsonSuffix(string mediaType)
    {
        var hasJsonSuffix = mediaType.EndsWith(JsonMediaTypeSuffix, StringComparison.InvariantCultureIgnoreCase);

        return hasJsonSuffix;
    }
}
