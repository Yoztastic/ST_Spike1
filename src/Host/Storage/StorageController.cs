using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageSpike.Application.Core.Storage;
using StorageSpike.Host.Common;
using StorageConstraintsRequest = StorageSpike.Host.Contracts.Requests.StorageConstraintsRequest;

namespace StorageSpike.Host.Storage;

[ApiController]
public class StorageController : DiscriminatingControllerBase
{
    private readonly StorageConstraintService _storageConstraintService;

    public StorageController(StorageConstraintService storageConstraintService) => _storageConstraintService = storageConstraintService;

    [HttpPost(ApiEndpoints.Storage.PostGetConstraints)]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetStorageConstraints([FromRoute] [Required] string dealId, [FromBody] StorageConstraintsRequest request )
    {
        var customerLanguage = await _storageConstraintService.GetStorageConstraints(dealId,  request.MapToDomain());
        return customerLanguage.Match(sr=>Ok(sr.MapToContract()), NotFound, ConstraintError);
    }
}
