using StorageSpike.Application.Core.DiscriminatedUnions;

namespace StorageSpike.Application.Core.Storage;

public class StorageConstraintService
{
    public async Task<Maybe<StorageConstraintsResponse,NotFound,ConstraintError>> GetStorageConstraints(string storageDealId, StorageConstraintsRequest mapToDomain)
    {
        throw new NotImplementedException();
    }
}
public class StorageConstraintsResponse
{
}

public class StorageConstraintsRequest
{
}

