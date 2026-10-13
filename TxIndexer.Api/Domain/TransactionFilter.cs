namespace TxIndexer.Api.Domain
{
    public record TransactionFilter(string? Token, string? Address, long? FromBlock, long? ToBlock, TransactionStatus? Status);
}
