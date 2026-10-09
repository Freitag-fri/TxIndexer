using System.Globalization;
using TxIndexer.Api.Contracts;
using TxIndexer.Api.Domain;

namespace TxIndexer.Api.Services
{
    public class TransactionService : ITransactionService
    {
        private const int MaxPageSize = 100;
        private readonly ITransactionRepository _repository;
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _repository = transactionRepository;
        }

        public async Task<PagedResponse<TransactionResponse>> GetPageAsync(int page, int pageSize, CancellationToken ct)
        {
            var actualPage = Math.Max(page, 1);
            var actualPageSize = Math.Clamp(pageSize, 1, MaxPageSize);

            var (items, totalCount) = await _repository.GetPageTransactionsAsync(actualPage, actualPageSize, ct);
            var responses = items.Select(n => ConvertTransactionToTransactionResponse(n)).ToList();

            return new PagedResponse<TransactionResponse>(responses, actualPage, actualPageSize, totalCount);
        }

        public async Task<TransactionResponse?> GetTransactionByHashAsync(string hash, CancellationToken ct)
        {
            var transaction = await _repository.GetTransactionByHashAsync(hash, ct);
            if (transaction is null)
            {
                return null;
            }

            var transactionResponse = ConvertTransactionToTransactionResponse(transaction);
            return transactionResponse;
        }

        private TransactionResponse ConvertTransactionToTransactionResponse(Transaction transaction)
        {
            var transactionResponse = new TransactionResponse(
                transaction.Hash,
                transaction.BlockNumber,
                transaction.From,
                transaction.To,
                transaction.Amount.ToString(CultureInfo.InvariantCulture),
                transaction.Token,
                transaction.Timestamp,
                transaction.Status.ToString()
            );
            return transactionResponse;
        }
    }
}
