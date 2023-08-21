namespace StorageSpike.Host.Middleware.Headers;

public static class StandardHeaders
{
    public static readonly ICollection<KeyValuePair<string, string[]>> SecurityHeaders =
        new List<KeyValuePair<string, string[]>>
        {
            new("X-Content-Type-Options", new[] {"nosniff"}),
            new("Strict-Transport-Security", new[] {"max-age=31536000"})
        };
}
