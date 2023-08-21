using System.Runtime.Serialization;
using System.Security.Permissions;

namespace StorageSpike.Host.Common.ServiceErrors;

public abstract class ServiceException : Exception
{
    private readonly List<Error> _errors = new();

    protected ServiceException(string code, string message, Severity severity, Exception innerException) : base(message, innerException)
    {
        Code = code;
        Severity = severity;
    }

    protected ServiceException(string code, string message, Severity severity) : base(message)
    {
        Code = code;
        Severity = severity;
    }

    public string Code { get; }

    public Severity Severity { get; }

    public List<Error> Errors => _errors;

    /// <exception cref="ArgumentNullException"></exception>
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null) throw new ArgumentNullException(nameof(info), "You must supply a non-null SerializationInfo property.");

        info.AddValue(nameof(Code), Code);
        info.AddValue(nameof(Severity), Severity);
        info.AddValue(nameof(Errors), Errors);

        base.GetObjectData(info, context);
    }
}
