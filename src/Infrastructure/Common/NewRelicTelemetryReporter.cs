using NewRelic.Api.Agent;
using NewRelicAgent = NewRelic.Api.Agent.NewRelic;

namespace Infrastructure.Common;

public class NewRelicTelemetryReporter : ITelemetryReporter
{
    private readonly IAgent Agent = NewRelicAgent.GetAgent();

    public void Report(Dictionary<string, object?> dictionary)
    {
        foreach (var keyValuePair in dictionary) Report(keyValuePair.Key, keyValuePair.Value);
    }

    public void Report(string key, object? value)
    {
        if(value!=null)
             Agent.CurrentTransaction.AddCustomAttribute(key,value);
    }
}

public interface ITelemetryReporter
{
    void Report(Dictionary<string, object?> dictionary);
    void Report(string key, object? value);
}
