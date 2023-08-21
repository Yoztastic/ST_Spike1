namespace StorageSpike.Host.Common.ServiceErrors;

public class Error
{
    private Error(string code, string detail)
    {
        Code = code;
        Detail = detail;
    }

    public Error() { }

    public Error(Severity severity, string code, string detail) : this(code, detail) =>
        Meta = new Metadata
        {
            Severity = severity
        };

    public Error(Severity severity, string code, string detail, string service) : this(severity, code, detail) => Meta.Service = service;

    public string Code { get; set; }
    public string Detail { get; set; }
    public Metadata Meta { get; set; }

}
