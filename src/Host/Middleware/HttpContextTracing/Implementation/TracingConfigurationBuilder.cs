namespace StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

internal class TracingConfigurationBuilder<THttpContext, TApplicationBuilder>
{
    public delegate TApplicationBuilder ConfigurationBuilt(TracingConfiguration<THttpContext> configuration);

    private readonly TracingConfiguration<THttpContext> _configuration;
    private readonly ConfigurationBuilt _configurationBuiltAction;

    internal TracingConfigurationBuilder(ConfigurationBuilt configurationBuiltAction)
    {
        _configurationBuiltAction = configurationBuiltAction;
        _configuration = new TracingConfiguration<THttpContext>();
    }

    internal TracingConfigurationBuilder<THttpContext, TApplicationBuilder> WithoutHeaders()
    {
        _configuration.CaptureHeaders = false;

        return this;
    }

    internal TracingConfigurationBuilder<THttpContext, TApplicationBuilder> WithoutBody()
    {
        _configuration.CaptureBody = false;

        return this;
    }

    internal TracingConfigurationBuilder<THttpContext, TApplicationBuilder> WithExclusions(Predicate<THttpContext>[]? exclusions)
    {
        _configuration.Exclusions = exclusions ?? Array.Empty<Predicate<THttpContext>>();

        return this;
    }

    internal TracingConfigurationBuilder<THttpContext, TApplicationBuilder> WithFieldMasking(string[]? fieldsToMask)
    {
        var toMask = fieldsToMask ?? Enumerable.Empty<string>();

        _configuration.MaskedFields = new HashSet<string>(toMask.Union(DefaultMaskedFields.All), StringComparer.InvariantCultureIgnoreCase);

        return this;
    }

    internal TracingConfigurationBuilder<THttpContext, TApplicationBuilder> WithoutFields(params string[]? excludedFields)
    {
        var toExclude = excludedFields ?? Enumerable.Empty<string>();

        _configuration.ExcludedFields = new HashSet<string>(toExclude, StringComparer.InvariantCultureIgnoreCase);

        return this;
    }

    internal TracingConfigurationBuilder<THttpContext, TApplicationBuilder> WithoutQueryString()
    {
        _configuration.CaptureQueryString = false;
        return this;
    }

    internal TracingConfigurationBuilder<THttpContext, TApplicationBuilder> WithWhitelistedHeadersOnly(params string[]? headerNames)
    {
        _configuration.CaptureWhitelistedHeadersOnly = true;

        var whitelistedHeaders = headerNames ?? Enumerable.Empty<string>();

        _configuration.WhitelistedHeaders = new HashSet<string>(whitelistedHeaders.Union(DefaultWhitelistedHeaders.All), StringComparer.InvariantCultureIgnoreCase);

        return this;
    }

    internal TracingConfigurationBuilder<THttpContext, TApplicationBuilder> WithBodyParser<TBodyParser>()
        where TBodyParser : IBodyParser, new()
    {
        return WithBodyParser(new TBodyParser());
    }

    public TracingConfigurationBuilder<THttpContext, TApplicationBuilder> WithBodyParser(IBodyParser bodyParser)
    {
        _configuration.BodyParsers.Add(bodyParser);

        return this;
    }

    internal TApplicationBuilder Use()
    {
        AddDefaultBodyParserIfNoneIsDefined();

        return _configurationBuiltAction(_configuration);
    }


    private void AddDefaultBodyParserIfNoneIsDefined()
    {
        if (_configuration.BodyParsers.Any()) return;

        _configuration.BodyParsers.Add(new JsonBodyParser());
    }
}
