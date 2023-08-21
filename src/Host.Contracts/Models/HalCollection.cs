namespace StorageSpike.Host.Contracts.Models;

public class HalCollection<T> : HalProperty
{
    public T[]? Values { get; set; }
}
