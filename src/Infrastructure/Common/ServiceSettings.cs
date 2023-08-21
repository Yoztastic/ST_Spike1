namespace Infrastructure.Common;

public class InfrastructureServiceSettings
{
    public string UserAgent => $"{SERVICE_NAME}/{SERVICE_VERSION}";

    public string SERVICE_NAME { get; set; } = "serviceName";

    public string SERVICE_VERSION { get; set; } = "serviceVersion";
}
