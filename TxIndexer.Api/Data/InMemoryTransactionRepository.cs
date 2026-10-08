//using System;
using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Threading.Tasks;
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

        public Task<(IReadOnlyList<Transaction>, int)> GetPageTransactionsAsync(int page, int pageSize, CancellationToken ct)
        {
            IReadOnlyList<Transaction> items = _transactions
                .OrderByDescending(t => t.Value.Timestamp)
                .ThenBy(t => t.Value.Hash)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => t.Value)
                .ToList();
            return Task.FromResult((items, _transactions.Count));
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
