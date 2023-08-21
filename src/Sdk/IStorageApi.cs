using Refit;
using StorageSpike.Host.Contracts.Models;
using StorageSpike.Host.Contracts.Requests;
using StorageSpike.Host.Contracts.Responses;

namespace Sdk;

public interface IStorageApi
{
    [Post(ApiEndpoints.Storage.PostGetConstraints)]
    Task<ApiResponse<StorageConstraintsResponse>> GetStorageConstraints(string storageId, StorageConstraintsRequest request);
}
