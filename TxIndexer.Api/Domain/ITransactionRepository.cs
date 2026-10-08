namespace TxIndexer.Api.Domain
{
    public interface ITransactionRepository
    {
        public Task<Transaction?> GetTransactionByHashAsync(string transactionHash, CancellationToken ct);

        public Task<(IReadOnlyList<Transaction>, int)> GetPageTransactionsAsync(int page, int pageSize, CancellationToken ct);

        public Task<bool> AddTransactionAsync(Transaction transaction, CancellationToken ct);
        public Task<bool> UpdateTransactionAsync(Transaction transaction, CancellationToken ct);
    }
}
