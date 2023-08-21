namespace StorageSpike.Host.Contracts.Models;

public class HalProperty
{
    public string? Href { get; init; }
}

public class HalProperty<T> : HalProperty
{
    public T? Value { get; set; }
}
