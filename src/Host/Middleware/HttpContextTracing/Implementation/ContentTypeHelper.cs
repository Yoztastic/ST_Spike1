namespace StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

internal static class ContentTypeHelper
{
    private static readonly string[] ContentTypeSeparators = {";"};
    private static readonly string[] MediaTypeSeparators = {","};

    internal static IEnumerable<string> GetMediaTypes(string contentType)
    {
        var mediaTypesDirective = GetMediaTypesDirective(contentType);
        if (mediaTypesDirective == null)
            return Enumerable.Empty<string>();

        var mediaTypes = mediaTypesDirective
            .Split(MediaTypeSeparators, StringSplitOptions.RemoveEmptyEntries)
            .Select(mediaType => mediaType.Trim());

        return mediaTypes;
    }

    private static string? GetMediaTypesDirective(string contentType)
    {
        var mediaTypesDirective = contentType
            .Split(ContentTypeSeparators, StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault();

        return mediaTypesDirective?.Trim();
    }
}
