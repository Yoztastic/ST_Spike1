
namespace StorageSpike.Application.Core;

public class NotFound
{
    public string Message { get; }

    internal NotFound(string message) => Message = message;

    public static NotFound By<T>(string name) => new($"'{typeof(T).Name}' not found with by '{name}'");
    public static NotFound ById<T>(string id) => new($"'{typeof(T).Name}' not found by id'{id}'");
    public static NotFound ById<T>(Guid id) => new($"'{typeof(T).Name}' not found by id '{id.ToString("D")}'");
    public static NotFound ById<T>(long id) => new($"'{typeof(T).Name}' not found by id '{id}'");
    public static NotFound Create<T>() => new($"'{typeof(T).Name}' not found");
}
