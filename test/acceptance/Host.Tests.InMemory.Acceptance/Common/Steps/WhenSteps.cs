
using StorageSpike.Acceptance.Helpers;

namespace StorageSpike.Host.Tests.InMemory.Acceptance.Common.Steps;

public class WhenSteps
{
    public WhenSteps(HostTestContext context) => Context = context;

    public Task<IApiResponse<T>> IGet<T>(string location, Action<HostTestContext>? opt = null!)
    {
        opt?.Invoke(Context);
        return Context.ApiClient.GetAsync<T>(location);
    }

    public Task<IApiResponse<TResponse>> IPost<TRequest, TResponse>(TRequest request, string location
        , ContentNegotiation? contentNegotiation = null, Action<HostTestContext>? opt = null!) where TRequest : class =>
        Send<TRequest, TResponse>(request, location, HttpMethod.Post, opt,contentNegotiation);

    private Task<IApiResponse<TResponse>> Send<TRequest, TResponse>(TRequest request, string location, HttpMethod method,
        Action<HostTestContext>? opt = null!, ContentNegotiation? contentNegotiation = null) where TRequest : class
    {
        opt?.Invoke(Context);
        return Context.ApiClient.SendRequest<object, TResponse>(method, location, request, contentNegotiation);
    }


    private HostTestContext Context { get; }

}
