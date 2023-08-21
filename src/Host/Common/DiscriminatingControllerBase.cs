using Microsoft.AspNetCore.Mvc;
using StorageSpike.Application.Core;
using StorageSpike.Application.Core.Kernel;
using StorageSpike.Host.Common.ServiceErrors;

namespace StorageSpike.Host.Common;

public class DiscriminatingControllerBase : ControllerBase
{
    protected IActionResult NotFound(NotFound n) => base.NotFound(new ErrorResponse {Errors = {new Error(Severity.Correctable, ErrorCodes.NotFound, n.Message)}});

    protected Func<ConstraintError, IActionResult> ConstraintError => invalid =>
        BadRequest(new ErrorResponse {Errors = {new Error(Severity.Correctable, ErrorCodes.InvalidOperation, invalid.Message)}});
}
