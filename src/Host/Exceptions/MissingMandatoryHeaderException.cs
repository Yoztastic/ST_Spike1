namespace StorageSpike.Host.Exceptions;

internal class MissingMandatoryHeaderException : Exception
{
    public string MandatoryHeader { get; }

    public MissingMandatoryHeaderException(string mandatoryHeader) =>
        MandatoryHeader = mandatoryHeader;
}
