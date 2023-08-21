namespace StorageSpike.Host.Common.ServiceErrors;

public class Metadata
{
    public Severity Severity { get; set; }
    public string Service { get; set; }

    public List<Link> Links { get; set; }

    public List<Error> InnerErrors { get; set; }
}
