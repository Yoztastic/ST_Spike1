namespace StorageSpike.Application.Core;

public interface ISerialisePolymorphically
{
    string TypeDiscriminator { get; }
}
