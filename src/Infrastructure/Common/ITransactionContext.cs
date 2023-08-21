namespace Infrastructure.Common;

public interface ITransactionContext
{
    public Dictionary<string,string> Headers { get; }
}