namespace StorageSpike.Application.Core;

public class ConstraintError
{
    public string Message { get; }

    public ConstraintError(string message) => Message = message;
}
