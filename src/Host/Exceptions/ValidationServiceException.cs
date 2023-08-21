using StorageSpike.Host.Common.ServiceErrors;

namespace StorageSpike.Host.Exceptions;

[Serializable]
public class ValidationServiceException : ServiceException
{
    public ValidationServiceException(string message) : base("InvalidRequest", message, Severity.Correctable) { }
}
