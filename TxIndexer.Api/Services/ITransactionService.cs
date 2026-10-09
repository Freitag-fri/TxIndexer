using TxIndexer.Api.Contracts;

namespace TxIndexer.Api.Services
{
    public interface ITransactionService
    {
        public Task<TransactionResponse?> GetTransactionByHashAsync(string hash, CancellationToken ct);
        public Task<PagedResponse<TransactionResponse>> GetPageAsync(int page, int pageSize, CancellationToken ct);
    }
}
