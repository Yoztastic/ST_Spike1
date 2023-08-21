using StorageSpike.Application.Core.Kernel;

namespace Application.Core.UnitTests;

public class TestMockTelemetry : ITelemetry
{
    public Dictionary<string, object?> Dictionary { get; } = new();
    public void Report(IDictionary<string, object?> dictionary)
    {
        foreach (var keyValuePair in dictionary) Report(keyValuePair.Key, keyValuePair.Value);
    }

    public void Report(string key, object? value)
    {
        if (!Dictionary.TryAdd(key,value)) Dictionary[key] = value;
    }
}


