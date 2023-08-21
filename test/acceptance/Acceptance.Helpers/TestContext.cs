
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StorageSpike.Acceptance.Helpers;

public class HostTestContext
{
    public ApiClient ApiClient { get; set; } = null!;
    public string ContextIdentity { get; set; }
    public string ConversationId { get; set; }
    public Uri BaseAddress { get; set; } = null!;

    public HostTestContext()
    {
        ContextIdentity = "who is calling me";
        ConversationId = Guid.NewGuid().ToString();
    }
}

public class ConsoleTraceWriter : ITraceWriter
{
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}
public class ApiClientConfiguration
{
    public ITraceWriter TraceWriter { get; set; }

    public ContractPropertyCase ContractPropertyCase { get; set; }

    public bool IncludeHostHeaderOnRequests { get; set; }

    public static ApiClientConfiguration Default = new ApiClientConfiguration
    {
        TraceWriter = new ConsoleTraceWriter(),
        ContractPropertyCase = ContractPropertyCase.None
    };

    public static ApiClientConfiguration WithHostHeaderSetOnRequests = new ApiClientConfiguration
    {
        TraceWriter = new ConsoleTraceWriter(),
        ContractPropertyCase = ContractPropertyCase.None,
        IncludeHostHeaderOnRequests = true
    };
}
public interface ITraceWriter
{
    void WriteLine(string message = null);
}

public class ContentNegotiation
{
    public string ContentType { get; set; }
    public string Accept { get; set; }

    public readonly Dictionary<string, string> Headers = new Dictionary<string, string>();
}

public interface ISerializer
{
    string Serialize<TRequest>(TRequest request);
    void TryDeserialize<TResponse>(string stringContent, out TResponse content);
}

public static class SerializationFactory
{
    public static ISerializer GetSerializer(string contentType, ContractPropertyCase contractPropertyCase)
    {
        // will we ever need this?
        /*switch (contentType)
        {
            case "application/xml":
            case "text/xml":
                return new XmlSerializer();
            case "application/x-www-form-urlencoded":
                return new FormSerializer();
        }*/

        return new CommonJsonSerializer(contractPropertyCase);
    }
}

public class CommonJsonSerializer : ISerializer
{
    private readonly ContractPropertyCase _contractPropertyCase;
    private readonly JsonSerializerOptions  _options;

    public CommonJsonSerializer(ContractPropertyCase contractPropertyCase)
    {
        _contractPropertyCase = contractPropertyCase;

        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = _contractPropertyCase == ContractPropertyCase.CamelCase
                ? JsonNamingPolicy.CamelCase
                : null,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        _options.Converters.Add(new JsonStringEnumConverter());
    }

    public string Serialize<TRequest>(TRequest request)
    {
        return JsonSerializer.Serialize(request,_options);
    }

    public void TryDeserialize<TResponse>(string stringContent, out TResponse content)
    {
        DeserializationHelper.TryGetContentOf(stringContent, out content, _contractPropertyCase);
    }
}

public class ApiClient : IDisposable
    {
        private readonly HttpClient _client;
        private readonly ApiClientConfiguration _configuration;
        private readonly ITraceWriter _tracerWriter;
        private readonly ContractPropertyCase _contractPropertyCase;

        public ApiClient() : this(new HttpClient(), ApiClientConfiguration.Default) { }

        public ApiClient(HttpClient httpClient, ApiClientConfiguration configuration)
        {
            _client = httpClient;
            _configuration = configuration;
            _tracerWriter = configuration.TraceWriter;
            _contractPropertyCase = configuration.ContractPropertyCase;
        }

        public void SetDefaultRequestHeaders(string name, string value)
        {
            _client.DefaultRequestHeaders.Remove(name);
            _client.DefaultRequestHeaders.Add(name, value);
        }

        public void RemoveDefaultRequestHeader(string name)
        {
            _client.DefaultRequestHeaders.Remove(name);
        }

        public void ClearDefaultRequestHeader()
        {
            _client.DefaultRequestHeaders.Clear();
        }

        public IApiResponse<string> Get(string uri, ContentNegotiation contentNegotiation = null)
        {
            return SendRequest(HttpMethod.Get, uri, null, contentNegotiation).Result;
        }

        public async Task<IApiResponse<string>> GetAsync(string uri, ContentNegotiation contentNegotiation = null)
        {
            return await SendRequest(HttpMethod.Get, uri, null, contentNegotiation);
        }

        public IApiResponse<TResponse> Get<TResponse>(string uri, ContentNegotiation contentNegotiation = null)
        {
            return SendRequest<object, TResponse>(HttpMethod.Get, uri, null, contentNegotiation).Result;
        }

        public async Task<IApiResponse<TResponse>> GetAsync<TResponse>(string uri,
            ContentNegotiation contentNegotiation = null)
        {
            return await SendRequest<object, TResponse>(HttpMethod.Get, uri, null, contentNegotiation);
        }

        public IApiResponse<string> Post(string uri, string request = null, ContentNegotiation contentNegotiation = null)
        {
            return SendRequest(HttpMethod.Post, uri, request, contentNegotiation).Result;
        }

        public Task<IApiResponse<string>> PostAsync(string uri, string request = null,
            ContentNegotiation contentNegotiation = null)
        {
            return SendRequest(HttpMethod.Post, uri, request, contentNegotiation);
        }

        public IApiResponse<TResponse> Post<TRequest, TResponse>(string uri, TRequest request = null,
            ContentNegotiation contentNegotiation = null)
            where TRequest : class
        {
            return SendRequest<TRequest, TResponse>(HttpMethod.Post, uri, request, contentNegotiation).Result;
        }

        public Task <IApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string uri, TRequest request = null,
            ContentNegotiation contentNegotiation = null)
            where TRequest : class
        {
            return SendRequest<TRequest, TResponse>(HttpMethod.Post, uri, request, contentNegotiation);
        }

        public Task <IApiResponse<TResponse>> PostFileAsync<TResponse>(string uri, MultipartFormDataContent formDataContent,
            ContentNegotiation contentNegotiation = null)
        {
            var requestMessage = GenerateRequestMessage(uri, formDataContent, contentNegotiation);

            return ProcessRequest(requestMessage, r => TryDeserializeResponse<TResponse>(r, contentNegotiation));
        }

        public IApiResponse<string> Put(string uri, string request = null,
            ContentNegotiation contentNegotiation = null)
        {
            return SendRequest(HttpMethod.Put, uri, request, contentNegotiation).Result;
        }

        public Task<IApiResponse<string>> PutAsync(string uri, string request = null,
            ContentNegotiation contentNegotiation = null)
        {
            return SendRequest(HttpMethod.Put, uri, request, contentNegotiation);
        }

        public IApiResponse<TResponse> Put<TRequest, TResponse>(string uri, TRequest request = null,
            ContentNegotiation contentNegotiation = null)
            where TRequest : class
        {
            return SendRequest<TRequest, TResponse>(HttpMethod.Put, uri, request, contentNegotiation).Result;
        }

        public Task<IApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string uri, TRequest request = null,
            ContentNegotiation contentNegotiation = null)
            where TRequest : class
        {
            return SendRequest<TRequest, TResponse>(HttpMethod.Put, uri, request, contentNegotiation);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing) _client?.Dispose();
            disposed = true;
        }

        public Task<IApiResponse<TResponse>> SendRequest<TRequest, TResponse>(HttpMethod method,
            string requestUri, TRequest request, ContentNegotiation contentNegotiation, ISerializer serializer)
            where TRequest : class
        {
            var content = CreateContent(request, serializer);

            var requestMessage = GenerateRequestMessage(method, requestUri, content, contentNegotiation);

            return ProcessRequest(requestMessage, r => TryDeserializeResponse<TResponse>(r, contentNegotiation));
        }

        public Task<IApiResponse<TResponse>> SendRequest<TRequest, TResponse>(HttpMethod method,
            string requestUri, TRequest request, ContentNegotiation contentNegotiation)
            where TRequest : class
        {
            var contentType = contentNegotiation?.ContentType;
            var serialization = SerializationFactory.GetSerializer(contentType, _contractPropertyCase);

            return SendRequest<TRequest, TResponse>(method, requestUri, request, contentNegotiation, serialization);
        }

        public async Task<IApiResponse<string>> SendRequest(HttpMethod method, string requestUri, string request,
            ContentNegotiation contentNegotiation)
        {
            var requestMessage = GenerateRequestMessage(method, requestUri, request, contentNegotiation);

            return await ProcessRequest(requestMessage, async responseMessage =>
            {
                var stringContent = await responseMessage.Content.ReadAsStringAsync();

                return stringContent;
            });
        }

        private async Task<IApiResponse<TResponse>> ProcessRequest<TResponse>(HttpRequestMessage request, Func<HttpResponseMessage, Task<TResponse>> postProcessor)
        {
            TraceRequest(request);

            var watch = Stopwatch.StartNew();

            var responseMessage = await _client.SendAsync(request);

            watch.Stop();

            var apiResponse = await BuildResponse(postProcessor, responseMessage);

            TraceResponse(responseMessage, apiResponse, watch.Elapsed);

            return apiResponse;
        }

        private async Task<TResponse> TryDeserializeResponse<TResponse>(HttpResponseMessage responseMessage, ContentNegotiation contentNegotiation)
        {
            var stringContent = await responseMessage.Content.ReadAsStringAsync();

            if (typeof(TResponse) == typeof(string))
            {
                return (TResponse)(object)stringContent;
            }

            var contentType = contentNegotiation?.ContentType;
            var serializer = SerializationFactory.GetSerializer(contentType, _contractPropertyCase);
            serializer.TryDeserialize(stringContent, out TResponse responseContent);

            return responseContent;
        }

        private static async Task<IApiResponse<TResponse>> BuildResponse<TResponse>(Func<HttpResponseMessage, Task<TResponse>> postProcessor, HttpResponseMessage responseMessage)
        {
            var stringContent = await responseMessage.Content.ReadAsStringAsync();

            var responseContent = await postProcessor(responseMessage);

            return ApiResponse<TResponse>.FromResponseMessage(responseMessage, responseContent, stringContent);
        }

        private static string CreateContent<TRequest>(TRequest request, ISerializer serializer)
            where TRequest : class
        {
            return request == null ? null : serializer.Serialize(request);
        }

        private HttpRequestMessage GenerateRequestMessage(string requestUri, MultipartFormDataContent formDataContent, ContentNegotiation contentNegotiation)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            ApplyContentNegotiation(requestMessage, contentNegotiation);

            requestMessage.Content = formDataContent;

            return requestMessage;
        }

        private HttpRequestMessage GenerateRequestMessage(HttpMethod method, string requestUri, string stringContent,
            ContentNegotiation contentNegotiation)
        {
            var requestMessage = new HttpRequestMessage(method, requestUri);

            ApplyRequestHeaders(requestMessage);
            ApplyContentNegotiation(requestMessage, contentNegotiation);

            var contentType = contentNegotiation?.ContentType;

            requestMessage.Content = string.IsNullOrEmpty(stringContent)
                ? null
                : new StringContent(stringContent, Encoding.UTF8, contentType ?? "application/json");

            return requestMessage;
        }

        private void ApplyContentNegotiation(HttpRequestMessage requestMessage, ContentNegotiation contentNegotiation)
        {
            if (null == contentNegotiation) return;

            if (!string.IsNullOrEmpty(contentNegotiation.Accept))
            {
                requestMessage.Headers.Remove("Accept");
                requestMessage.Headers.Add("Accept", contentNegotiation.Accept);
            }

            foreach (var header in contentNegotiation.Headers)
            {
                requestMessage.Headers.Remove(header.Key);
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        private void ApplyRequestHeaders(HttpRequestMessage requestMessage)
        {
            if (_configuration.IncludeHostHeaderOnRequests)
            {
                requestMessage.Headers.Host = requestMessage.RequestUri.IsAbsoluteUri
                    ? requestMessage.RequestUri.Authority
                    : _client.BaseAddress.Authority;
            }
        }

        private void TraceRequest(HttpRequestMessage requestMessage)
        {
            _tracerWriter.WriteLine();
            _tracerWriter.WriteLine(">>> BEGIN REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            _tracerWriter.WriteLine($"[{requestMessage.Method}] {requestMessage.RequestUri}");

            _tracerWriter.WriteLine("-----------------------------------------------");
            _tracerWriter.WriteLine(requestMessage.Content?.ReadAsStringAsync().Result);

            WriteHeaders(_client.DefaultRequestHeaders, requestMessage.Headers, requestMessage.Content?.Headers);

            _tracerWriter.WriteLine(">>> END REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            _tracerWriter.WriteLine();
        }

        private void TraceResponse(HttpResponseMessage responseMessage, IApiResponse response, TimeSpan responseTime)
        {
            _tracerWriter.WriteLine();
            _tracerWriter.WriteLine("<<< BEGIN RESPONSE <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");

            _tracerWriter.WriteLine($"[{(int)response.StatusCode}] {response.StatusCode} from {responseMessage?.RequestMessage?.RequestUri}");

            _tracerWriter.WriteLine("-----------------------------------------------");

            _tracerWriter.WriteLine($"Request took {responseTime.TotalSeconds:##.#} seconds.");

            _tracerWriter.WriteLine("-----------------------------------------------");

            _tracerWriter.WriteLine(response.StringContent);

            WriteHeaders(responseMessage.Headers, responseMessage.Content.Headers);

            _tracerWriter.WriteLine("<<< END RESPONSE <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            _tracerWriter.WriteLine();
        }

        private void WriteHeaders(params HttpHeaders[] httpHeaders)
        {
            var printableHeaders = httpHeaders
                .Where(x => x != null)
                .SelectMany(x => x.Select(AsPrintableHeader))
                .ToArray();

            if (!printableHeaders.Any()) return;

            _tracerWriter.WriteLine("-----------------------------------------------");
            _tracerWriter.WriteLine(string.Join(Environment.NewLine, printableHeaders));
        }

        private static string AsPrintableHeader(KeyValuePair<string, IEnumerable<string>> header)
        {
            var value = string.Join(";", header.Value);
            return $"{header.Key}={value}";
        }
    }

 public interface IApiResponse
    {
        HttpStatusCode StatusCode { get; }
        HeaderCollection Headers { get; }
        HeaderCollection ContentHeaders { get; }
        string StringContent { get; }
        T GetContentOf<T>(ContractPropertyCase contractPropertyCase = ContractPropertyCase.None);
        bool TryGetContentOf<TResponse>(out TResponse content, ContractPropertyCase contractPropertyCase = ContractPropertyCase.None);
        bool IsSuccessStatusCode { get; }
    }

    public interface IApiResponse<out T> : IApiResponse
    {
        //The interface is necessary to support covariance
        T Content { get; }
    }

    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public HeaderCollection Headers { get; set; }

        public HeaderCollection ContentHeaders { get; set; }

        public string StringContent { get; set; }

        public T GetContentOf<T>(ContractPropertyCase contractPropertyCase = ContractPropertyCase.None)
        {
            return DeserializationHelper.GetContentOf<T>(StringContent, contractPropertyCase);
        }

        public bool TryGetContentOf<T>(out T content, ContractPropertyCase contractPropertyCase = ContractPropertyCase.None)
        {
            return DeserializationHelper.TryGetContentOf<T>(StringContent, out content, contractPropertyCase);
        }

        public bool IsSuccessStatusCode
        {
            get
            {
                if (StatusCode >= HttpStatusCode.OK)
                {
                    return StatusCode <= (HttpStatusCode) 299;
                }
                return false;
            }
        }
    }

public static class DeserializationHelper
{
    public static T GetContentOf<T>(string value, ContractPropertyCase contractPropertyCase = ContractPropertyCase.None)
    {
        var jsonSerializerSettings = CreateSerializerOptions(contractPropertyCase);

        return JsonSerializer.Deserialize<T>(value, jsonSerializerSettings);
    }

    public static bool TryGetContentOf<T>(string value, out T content, ContractPropertyCase contractPropertyCase = ContractPropertyCase.None)
    {
        var jsonSerializerSettings = CreateSerializerOptions(contractPropertyCase);

        try
        {
            content = JsonSerializer.Deserialize<T>(value, jsonSerializerSettings);
            return true;
        }
        catch
        {
            content = default;
            return false;
        }
    }

    private static JsonSerializerOptions  CreateSerializerOptions (ContractPropertyCase contractPropertyCase)
    {
        var jsonSerializerOptions = new JsonSerializerOptions();

        if (contractPropertyCase == ContractPropertyCase.CamelCase)
        {
            jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }
        else if (contractPropertyCase == ContractPropertyCase.PascalCase)
        {
            jsonSerializerOptions.PropertyNamingPolicy = null; // default behavior for PascalCase
        }

        return jsonSerializerOptions;
    }
}

    public class ApiResponse<T> : ApiResponse, IApiResponse<T>
    {
        public T Content { get; private set; }

        public static IApiResponse<T> FromResponseMessage(HttpResponseMessage responseMessage, T content, string stringContent)
        {
            var statusCode = responseMessage.StatusCode;
            var headers = new HeaderCollection(responseMessage.Headers);
            var contentHeaders = new HeaderCollection(responseMessage.Content.Headers);

            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Headers = headers,
                ContentHeaders = contentHeaders,
                StringContent = stringContent,
                Content = content
            };
        }
    }
public enum ContractPropertyCase
{
    None,
    CamelCase,
    PascalCase
}

public class HeaderCollection : List<Header>
{
    public HeaderCollection(IEnumerable<Header> collection) : base(collection)
    {
    }

    public HeaderCollection(HttpHeaders collection) : base(collection.Select(_ => new Header(_.Key, _.Value.FirstOrDefault())))
    {
    }

    public string this[string name]
    {
        get { return this.FirstOrDefault(h => h.Name == name)?.Value; }
    }
}

public class Header
{
    public string Name { get; private set; }
    public string Value { get; private set; }

    public Header(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Name}={Value}";
    }
}

