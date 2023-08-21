using System.Net;
using System.Net.Mime;
using System.Text;
using RichardSzalay.MockHttp;

namespace StorageSpike.Host.Tests.InMemory.Acceptance.Common.HttpMocking;

public static class MockHttpExtensions
{
    public static void Mock<TResponse>(this MockHttpMessageHandler mockHttpMessageHandler,
        string url,
        Func<HttpRequestMessage, Task<string?>> response,
        HttpMethod? method = null,
        HttpStatusCode code = HttpStatusCode.OK,
        int delayInMs = 0,
        List<KeyValuePair<string, string>>? headers = null,
        string mediaType = MediaTypeNames.Application.Json,
        HttpStatusCode? missingCode = null,
        IMockedRequestMatcher? matcher = null,
        string? accept = null)
    {
        var mockedRequestMatcher = matcher ?? (accept == null ? new AlwaysTrueMatcher() : new AcceptMatcher(accept));
        MockBase<TResponse>(mockHttpMessageHandler, url, response, method, code, delayInMs, headers, missingCode,
            mockedRequestMatcher, r => new StringContent(r!, Encoding.UTF8, mediaType));
    }


    public static void MockBytes(this MockHttpMessageHandler mockHttpMessageHandler,
        string url,
        Func<HttpRequestMessage, Task<string?>> response,
        HttpMethod? method = null,
        HttpStatusCode code = HttpStatusCode.OK,
        int delayInMs = 0,
        Dictionary<string, string>? headers = null,
        HttpStatusCode? missingCode = null)
    {
        MockBase<byte[]>(mockHttpMessageHandler, url, response, method, code, delayInMs, headers, missingCode,
            new AlwaysTrueMatcher(), r => new ByteArrayContent(Encoding.UTF8.GetBytes(r!)));
    }

    private static void MockBase<TResponse>(this MockHttpMessageHandler mockHttpMessageHandler,
        string url,
        Func<HttpRequestMessage, Task<string?>> response,
        HttpMethod? method,
        HttpStatusCode code,
        int delayInMs,
        IEnumerable<KeyValuePair<string, string>>? headers,
        HttpStatusCode? missingCode,
        IMockedRequestMatcher mockedRequestMatcher,
        Func<string?, HttpContent> contentFunction)
    {

        mockHttpMessageHandler.When(method ?? HttpMethod.Get, url).With(mockedRequestMatcher).Respond(async request =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(delayInMs));

            var responseBody = await response(request);
            if (responseBody == null && missingCode != null)
            {
                return new HttpResponseMessage(missingCode.Value);
            }

            var message = new HttpResponseMessage(code);

            if (typeof(TResponse) != typeof(EmptyResponse))
            {
                message.Content = contentFunction(responseBody);
            }

            if (headers != null)
            {
                foreach (var (key, value) in headers)
                {
                    message.Headers.Add(key, value);
                }
            }

            return message;
        });
    }
}
