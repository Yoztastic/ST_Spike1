using StorageSpike.Host.Contracts.Requests;
using StorageSpike.Host.Contracts.Responses;

namespace StorageSpike.Host.Storage;

public static class StorageMapping
{
    public static Application.Core.Storage.StorageConstraintsRequest MapToDomain(this StorageConstraintsRequest request)
    {
        return new Application.Core.Storage.StorageConstraintsRequest();
    }

    public static StorageConstraintsResponse MapToContract(this Application.Core.Storage.StorageConstraintsResponse request)
    {
        return new StorageConstraintsResponse();
    }
}
