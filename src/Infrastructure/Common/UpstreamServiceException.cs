using System.Net;
using StorageSpike.Application.Core;

namespace Infrastructure.Common;

public class UpstreamServiceException : StorageSpikeException
{
    private static string Map(HttpResponseMessage httpResponseMessage)
    {
        var requestUri = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri ?? "Unknown Uri";

        var reason = httpResponseMessage.ReasonPhrase != null ? $" [{httpResponseMessage.ReasonPhrase}]" : string.Empty;

        return $"Unhandled Response from {requestUri}{reason}";
    }
    public UpstreamServiceException(HttpResponseMessage httpResponseMessage) : base(Map(httpResponseMessage))
    {
        StatusCode = httpResponseMessage.StatusCode;
    }

    public HttpStatusCode StatusCode { get; }

}
