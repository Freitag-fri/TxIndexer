using System.Globalization;
using TxIndexer.Api.Contracts;
using TxIndexer.Api.Domain;

namespace TxIndexer.Api.Services
{
    public class TransactionService : ITransactionService
    {
        public const int MaxPageSize = 100;
        private readonly ITransactionRepository _repository;
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _repository = transactionRepository;
        }

        public async Task<PagedResponse<TransactionResponse>> GetPageAsync(TransactionQuery transactionQuery, CancellationToken ct)
        {
            var actualPage = Math.Max(transactionQuery.Page, 1);
            var actualPageSize = Math.Clamp(transactionQuery.PageSize, 1, MaxPageSize);

            var transactionFilter = new TransactionFilter (
                transactionQuery.Token,
                transactionQuery.Address,
                transactionQuery.FromBlock,
                transactionQuery.ToBlock,
                transactionQuery.Status
            );

            var (items, totalCount) = await _repository.GetPageTransactionsAsync(transactionFilter, actualPage, actualPageSize, ct);
            var responses = items.Select(n => ConvertTransactionToTransactionResponse(n)).ToList();

            return new PagedResponse<TransactionResponse>(responses, actualPage, actualPageSize, totalCount);
        }

        public async Task<TransactionResponse?> GetTransactionByHashAsync(string hash, CancellationToken ct)
        {
            string lowerCaseHash = hash.ToLowerInvariant();
            var transaction = await _repository.GetTransactionByHashAsync(lowerCaseHash, ct);
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
