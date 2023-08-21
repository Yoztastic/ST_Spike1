namespace StorageSpike.Application.Core.Kernel;

public interface ITelemetry
{
    Dictionary<string, object?> Dictionary { get; }
    void Report(IDictionary<string,object?> dictionary);
    void Report(string key, object? value);
   //  void Report<T>(T t); could add something like this then inject a bunch of typed mappers
}

public class Telemetry : ITelemetry
{
    private readonly AsyncLocal<Dictionary<string, object?>> localDictionary = new();

    public Dictionary<string, object?> Dictionary => localDictionary.Value ?? (localDictionary.Value = new Dictionary<string, object?>());

    public void Report(IDictionary<string, object?> dictionary)
    {
        foreach (var keyValuePair in dictionary) Report(keyValuePair.Key, keyValuePair.Value);
    }

    public void Report(string key, object? value)
    {
        if (value!=null && !Dictionary.TryAdd(key,value)) Dictionary[key] = value; // overwrite existing;
    }
}
