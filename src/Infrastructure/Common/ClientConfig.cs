// ReSharper disable UnusedAutoPropertyAccessor.Global -- Config used by reflection
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace Infrastructure.Common;

public class ClientConfig<T> : ClientConfig
{
    public string SectionName => $"{typeof(T).Name}Config";
}

public class ClientConfig
{
    public string Host { get; set; }
    public int TimeOutMs { get; set; } = 500;
}
