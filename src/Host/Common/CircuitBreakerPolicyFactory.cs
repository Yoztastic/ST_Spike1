using System.Net;
using Polly;

namespace StorageSpike.Host.Common;

public static class CircuitBreakerPolicyFactory
{
    public static IAsyncPolicy<HttpResponseMessage> CreatePolicy(CircuitBreakerSettings circuitBreakerSettings, bool applyRetryPolicy = true, Func<int, TimeSpan>? waitAndRetryTimeFunc = null)
    {
        return CreateInnerPolicy(
            circuitBreakerSettings.BreakDuration,
            circuitBreakerSettings.FailureThreshold,
            circuitBreakerSettings.RetriesCount,
            applyRetryPolicy,
            waitAndRetryTimeFunc);
    }

    private static IAsyncPolicy<HttpResponseMessage> CreateInnerPolicy(TimeSpan circuitOpenDuration, int errorsBeforeCircuitTrips, int retries, bool applyRetryPolicy, Func<int, TimeSpan>? waitAndRetryTimeFunc)
    {
        bool IsTransientFault(HttpStatusCode statusCode) =>
            statusCode is HttpStatusCode.BadGateway or
                          HttpStatusCode.GatewayTimeout or
                          HttpStatusCode.RequestTimeout or
                          HttpStatusCode.ServiceUnavailable;

        var circuitBreakerPolicy = Policy
            .Handle<TaskCanceledException>()
            .Or<HttpRequestException>()
            .OrResult<HttpResponseMessage>(resp => IsTransientFault(resp.StatusCode))
            .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: errorsBeforeCircuitTrips, durationOfBreak: circuitOpenDuration);

        var retryPolicy = Policy
            .Handle<TaskCanceledException>()
            .Or<HttpRequestException>()
            .OrResult<HttpResponseMessage>(resp => IsTransientFault(resp.StatusCode))
            .WaitAndRetryAsync(retries, waitAndRetryTimeFunc ?? (i => TimeSpan.FromMilliseconds(i * 100)),
                onRetry: (result, _) =>
                {
                    result.Result?.Dispose();
                });

        if (applyRetryPolicy)
            return circuitBreakerPolicy.WrapAsync(retryPolicy);

        return circuitBreakerPolicy;
    }

}
