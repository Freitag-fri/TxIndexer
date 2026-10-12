//using System;
using System.Collections.Concurrent;
using TxIndexer.Api.Domain;



namespace TxIndexer.Api.Data
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        ConcurrentDictionary<string, Transaction> _transactions = new();
        public Task<bool> AddTransactionAsync(Transaction transaction, CancellationToken ct)
        {
            return Task.FromResult(_transactions.TryAdd(transaction.Hash, transaction));
        }

        public Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPageTransactionsAsync(TransactionFilter transactionFilter, int page, int pageSize, CancellationToken ct)
        {
            IEnumerable<Transaction> query = _transactions.Values;

            if (transactionFilter.Token is not null)
                query = query.Where(t => string.Equals(t.Token, transactionFilter.Token, StringComparison.OrdinalIgnoreCase));
            if (transactionFilter.FromBlock is not null)
                query = query.Where(t => t.BlockNumber >= transactionFilter.FromBlock);
            if (transactionFilter.ToBlock is not null)
                query = query.Where(t => t.BlockNumber <= transactionFilter.ToBlock);
            if (transactionFilter.Address is not null)
                query = query.Where(t => string.Equals(t.From == transactionFilter.Address, StringComparison.OrdinalIgnoreCase) || string.Equals(t.To == transactionFilter.Address, StringComparison.OrdinalIgnoreCase));
            if (transactionFilter.Status is not null)
                query = query.Where(t => t.Status == transactionFilter.Status);

            query = query.ToList();
            var transactionsCount = query.Count();
            IReadOnlyList<Transaction> items = query
                .OrderByDescending(t => t.Timestamp)
                .ThenBy(t => t.Hash)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return Task.FromResult((items, transactionsCount));
        }

        public Task<Transaction?> GetTransactionByHashAsync(string transactionHash, CancellationToken ct)
        {
            _transactions.TryGetValue(transactionHash, out var transaction);
            return Task.FromResult(transaction);
        }

        public Task<bool> UpdateTransactionAsync(Transaction transaction, CancellationToken ct)
        {
            if(_transactions.TryGetValue(transaction.Hash, out var existingTransaction))
            {
                return Task.FromResult(_transactions.TryUpdate(transaction.Hash, transaction, existingTransaction));
            }
            return Task.FromResult(false);
        }
    }
}
