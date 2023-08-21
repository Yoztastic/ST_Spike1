namespace StorageSpike.Host.Common.ServiceErrors;

public class Link
{
    public Link() { }

    public string Rel { get; set; }
    public string Href { get; set; }
    public LinkMetadata Meta { get; set; }
}
