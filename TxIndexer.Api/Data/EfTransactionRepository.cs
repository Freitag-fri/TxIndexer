using System.Collections.Concurrent;
using System.Reflection.Metadata.Ecma335;
using TxIndexer.Api.Domain;
using Microsoft.EntityFrameworkCore;


namespace TxIndexer.Api.Data
{
    public class EfTransactionRepository : ITransactionRepository
    {
        private readonly TxIndexerDbContext _txIndexerDbContext;
        public EfTransactionRepository(TxIndexerDbContext txIndexerDbContext)
        {
            _txIndexerDbContext = txIndexerDbContext;
        }
        public async Task<bool> AddTransactionAsync(Transaction transaction, CancellationToken ct)
        {
            if(await _txIndexerDbContext.Transactions.AnyAsync(t => t.Hash == transaction.Hash, ct))
            {
                return false;
            }

            _txIndexerDbContext.Transactions.Add(transaction);
            await _txIndexerDbContext.SaveChangesAsync(ct);
            return true;
        }

        public Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPageTransactionsAsync(TransactionFilter transactionFilter, int page, int pageSize, CancellationToken ct)
        {
            return null;
            //IEnumerable<Transaction> query = _transactions.Values;

            //if (transactionFilter.Token is not null)
            //    query = query.Where(t => string.Equals(t.Token, transactionFilter.Token, StringComparison.OrdinalIgnoreCase));
            //if (transactionFilter.FromBlock is not null)
            //    query = query.Where(t => t.BlockNumber >= transactionFilter.FromBlock);
            //if (transactionFilter.ToBlock is not null)
            //    query = query.Where(t => t.BlockNumber <= transactionFilter.ToBlock);
            //if (transactionFilter.Address is not null)
            //    query = query.Where(t => string.Equals(t.From, transactionFilter.Address, StringComparison.OrdinalIgnoreCase) || string.Equals(t.To, transactionFilter.Address, StringComparison.OrdinalIgnoreCase));
            //if (transactionFilter.Status is not null)
            //    query = query.Where(t => t.Status == transactionFilter.Status);

            //query = query.ToList();
            //var transactionsCount = query.Count();
            //IReadOnlyList<Transaction> items = query
            //    .OrderByDescending(t => t.Timestamp)
            //    .ThenBy(t => t.Hash)
            //    .Skip((page - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();
            //return Task.FromResult((items, transactionsCount));
        }

        public async Task<Transaction?> GetTransactionByHashAsync(string transactionHash, CancellationToken ct)
        {
            return await _txIndexerDbContext.Transactions.SingleOrDefaultAsync(t => t.Hash == transactionHash, ct);
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transaction, CancellationToken ct)
        {
            //if(_transactions.TryGetValue(transaction.Hash, out var existingTransaction))
            //{
            //    return Task.FromResult(_transactions.TryUpdate(transaction.Hash, transaction, existingTransaction));
            //}
            //return Task.FromResult(false);
            return false;
        }
    }
}
