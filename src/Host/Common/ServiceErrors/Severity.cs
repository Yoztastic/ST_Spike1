namespace StorageSpike.Host.Common.ServiceErrors;

[Serializable]
public enum Severity
{
    Correctable,
    Intermittent,
    Unexpected,
    Unrecoverable
}
