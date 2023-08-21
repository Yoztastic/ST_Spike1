namespace StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

internal class TracingConfiguration<THttpContext>
{
    internal bool CaptureBody { get; set; } = true;
    internal bool CaptureHeaders { get; set; } = true;
    internal bool CaptureWhitelistedHeadersOnly { get; set; } = false;
    internal bool CaptureQueryString { get; set; } = true;

    internal Predicate<THttpContext>[] Exclusions { get; set; } = Array.Empty<Predicate<THttpContext>>();
    internal ICollection<string> MaskedFields { get; set; } = new HashSet<string>(DefaultMaskedFields.All, StringComparer.InvariantCultureIgnoreCase);
    internal ICollection<string> WhitelistedHeaders { get; set; } = new HashSet<string>(DefaultWhitelistedHeaders.All, StringComparer.InvariantCultureIgnoreCase);
    internal List<IBodyParser> BodyParsers { get; set; } = new();
    internal HashSet<string> ExcludedFields { get; set; } = new(StringComparer.InvariantCulture);
}
