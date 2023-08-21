

namespace StorageSpike.Acceptance.Helpers;

public static class ApiClientFactory
{
    public static ApiClient For(HttpClient client)
    {
        var apiClientConfiguration = new ApiClientConfiguration { TraceWriter = new ConsoleTraceWriter() };

        var apiClient = new ApiClient(client, apiClientConfiguration);
        return apiClient;
    }
}
