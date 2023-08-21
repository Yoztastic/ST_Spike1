namespace StorageSpike.Host.Common.ServiceErrors;

public class ErrorResponse
{
    public List<Error> Errors { get; set; } = new List<Error>();
}
