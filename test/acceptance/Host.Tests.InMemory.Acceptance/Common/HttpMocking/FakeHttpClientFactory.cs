using System.Net;
using RichardSzalay.MockHttp;

namespace StorageSpike.Host.Tests.InMemory.Acceptance.Common.HttpMocking;

public class FakeHttpClientFactory : IHttpClientFactory
{
    private readonly MockHttpMessageHandler _mockHttpMessageHandler;

    public FakeHttpClientFactory(MockHttpMessageHandler mockHttpMessageHandler)
    {
        _mockHttpMessageHandler = mockHttpMessageHandler;
        _mockHttpMessageHandler.Fallback.Respond(req => new HttpResponseMessage(HttpStatusCode.NotFound));
    }

    public HttpClient CreateClient(string name)
    {
        // todo ideally get client settings from config and build an identical client to what was requested Like we did in TNS

        var httpClient = _mockHttpMessageHandler.ToHttpClient();

        httpClient.BaseAddress = new Uri("http://localhost/");

        return httpClient;
    }
}
