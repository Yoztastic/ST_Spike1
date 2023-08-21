namespace Infrastructure.Common;

public static class HttpContentExtensions
{
    public static async Task<T> ReadAsAsync<T>(this HttpContent content)
    {
        ArgumentNullException.ThrowIfNull(nameof(content));
        return (await System.Text.Json.JsonSerializer.DeserializeAsync<T>(await content.ReadAsStreamAsync()))!;
    }
}
