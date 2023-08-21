using System.Net;
using System.Net.Mime;
using RichardSzalay.MockHttp;
using StorageSpike.Host.Tests.InMemory.Acceptance.Common.HttpMocking;

namespace StorageSpike.Host.Tests.InMemory.Acceptance.Common.Steps;

public class InMemoryGivenSteps
{
    private readonly MockHttpMessageHandler _mockHttpMessageHandler;

    public InMemoryGivenSteps(MockHttpMessageHandler mockHttpMessageHandler)
    {
        _mockHttpMessageHandler = mockHttpMessageHandler;
    }

    public void AFreshSetOfMockedBehaviour()
    {
        _mockHttpMessageHandler.Clear();
    }

    public void AnExternalServiceResponds<TResponse>(
        string url,
        Func<HttpRequestMessage, Task<string?>> response,
        HttpMethod? method = null,
        HttpStatusCode code = HttpStatusCode.OK,
        int delayInMs = 0,
        List<KeyValuePair<string, string>>? headers = null,
        string mediaType = MediaTypeNames.Application.Json,
        HttpStatusCode? missingCode = null,
        string? accept = null)
    {
        _mockHttpMessageHandler.Mock<TResponse>(url, response, method ?? HttpMethod.Get, code, delayInMs, headers, mediaType, missingCode,null,accept);
    }
}
