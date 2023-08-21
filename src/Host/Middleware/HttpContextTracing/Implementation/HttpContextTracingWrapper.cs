using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using StorageSpike.Host.Common.Logging;

namespace StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

internal class HttpContextTracingWrapper : IDisposable
{
    private readonly TracingConfiguration<HttpContext> _configuration;
    private readonly HttpContext _context;
    private readonly Action<HttpContextTrace>[] _tracingActions;

    private readonly Stream _originalRequestStream;
    private readonly Stream _originalResponseStream;

    private readonly MemoryStream _memoryRequestStream;
    private readonly MemoryStream _memoryResponseStream;
    private readonly HttpRequestTrace _httpRequestTrace;
    private readonly HttpResponseTrace _httpResponseTrace;

    private readonly Stopwatch _stopwatch;
    private readonly DateTime _dateTimeUtc;

    internal HttpContextTracingWrapper(TracingConfiguration<HttpContext> configuration, HttpContext context, Action<HttpContextTrace>[] tracingActions)
    {
        _configuration = configuration;
        _context = context;
        _tracingActions = tracingActions;
        _originalRequestStream = context.Request.Body;
        _originalResponseStream = context.Response.Body;

        _memoryRequestStream = new MemoryStream();
        _memoryResponseStream = new MemoryStream();
        _httpRequestTrace = new HttpRequestTrace();
        _httpResponseTrace = new HttpResponseTrace();
        _stopwatch = new Stopwatch();
        _dateTimeUtc = DateTime.UtcNow;

        _context.Request.Body = _memoryRequestStream;
        _context.Response.Body = _memoryResponseStream;
    }

    internal async Task BeginAsync()
    {
        await _originalRequestStream.CopyToAsync(_memoryRequestStream);

        _httpRequestTrace.Method = _context.Request.Method;
        _httpRequestTrace.Path = _context.Request.Path.ToString();
        _httpRequestTrace.QueryString = GetQueryString(_context.Request.QueryString);
        _httpRequestTrace.Body = GetBody(_memoryRequestStream, _context.Request.ContentType);
        _httpRequestTrace.Headers = GetHeaders(_context.Request.Headers);

        _stopwatch.Start();

        _memoryRequestStream.Seek(0, SeekOrigin.Begin);
    }

    internal async Task EndAsync()
    {
        _stopwatch.Stop();

        _memoryResponseStream.Seek(0, SeekOrigin.Begin);

        await _memoryResponseStream.CopyToAsync(_originalResponseStream);

        _context.Response.Body = _originalResponseStream;

        _httpResponseTrace.StatusCode = _context.Response.StatusCode;
        _httpResponseTrace.Body = GetBody(_memoryResponseStream, _context.Response.ContentType);
        _httpResponseTrace.Headers = GetHeaders(_context.Response.Headers);
    }

    public void Dispose()
    {
        var httpContextTrace = new HttpContextTrace
        {
            Request = _httpRequestTrace,
            Response = _httpResponseTrace,
            Duration = _stopwatch.Elapsed,
            DateTimeUtc = _dateTimeUtc,
            Environment = new Dictionary<string, object>(),
            Identity = GetIdentityOrDefault()
        };

        foreach (var action in _tracingActions)
        {
            try
            {
                action(httpContextTrace);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"An error has occurred calling tracing actions: {ex.Message}");
            }
        }
    }

    private JsonDocument? GetBody(MemoryStream stream, string contentType)
    {
        if (!_configuration.CaptureBody || stream.Length == 0)
        {
            return null;
        }

        stream.Seek(0, SeekOrigin.Begin);

        var body = _configuration.BodyParsers
            .Where(parser => parser.CanParse(contentType))
            .Select(parser => ParseBodyOrDefault(parser, stream))
            .FirstOrDefault(x => x != null);

        return body?.ExcludeFields(_configuration.ExcludedFields)
            .Mask(_configuration.MaskedFields);
    }

    private static JsonDocument ParseBodyOrDefault(IBodyParser bodyParser, MemoryStream stream)
    {
        try
        {
            var body = bodyParser.Parse(stream);

            return body;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private Dictionary<string, string> GetHeaders(IHeaderDictionary headers)
    {
        var result = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

        if (!_configuration.CaptureHeaders) return result;

        foreach (var header in headers)
        {
            if (result.ContainsKey(header.Key))
                continue;

            if (_configuration.CaptureWhitelistedHeadersOnly
                && !_configuration.WhitelistedHeaders.Contains(header.Key))
                continue;

            result[header.Key] = string.Join(",", header.Value);
        }

        return result
            .Exclude(_configuration.ExcludedFields)
            .Mask(_configuration.MaskedFields);
    }

    private string GetIdentityOrDefault()
    {
        var identity = _context.User?.FindFirst(ClaimTypes.Name)?.Value;

        return identity;
    }

    private string GetQueryString(QueryString queryString)
    {
        if (!_configuration.CaptureQueryString || !queryString.HasValue) return string.Empty;

        var items = queryString.Value.Split('&')
            .Select(i => i.Split('='))
            .ToDictionary(k => k[0], v => v.Length == 2 ? v[1] : null);

        var resultingItems = items
            .ExcludeStripNullValue(_configuration.MaskedFields)
            .Mask(_configuration.MaskedFields);

        return $"?{string.Join("&", resultingItems.Select(i => i.Value != null ? $"{i.Key}={i.Value}" : i.Key))}";
    }
}
