namespace TxIndexer.Api.Contracts
{
    public record TransactionResponse(string Hash, long BlockNumber, string From, string To, string Amount, string Token, long Timestamp, string Status);
}
