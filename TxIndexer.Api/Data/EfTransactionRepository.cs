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

        public async Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPageTransactionsAsync(TransactionFilter transactionFilter, int page, int pageSize, CancellationToken ct)
        {
            IQueryable<Transaction> query = _txIndexerDbContext.Transactions.AsNoTracking();
            if (transactionFilter.Token is not null)
                query = query.Where(t => t.Token == transactionFilter.Token);
            if (transactionFilter.FromBlock is not null)
                query = query.Where(t => t.BlockNumber >= transactionFilter.FromBlock);
            if (transactionFilter.ToBlock is not null)
                query = query.Where(t => t.BlockNumber <= transactionFilter.ToBlock);
            if (transactionFilter.Address is not null)
                query = query.Where(t => t.From == transactionFilter.Address || t.To == transactionFilter.Address);
            if (transactionFilter.Status is not null)
                query = query.Where(t => t.Status == transactionFilter.Status);

            var totalCount = await query.CountAsync(ct);
            var items = await query
                .OrderByDescending(t => t.Timestamp)
                .ThenBy(t => t.Hash)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
            return (items, totalCount);
        }

        public async Task<Transaction?> GetTransactionByHashAsync(string transactionHash, CancellationToken ct)
        {
            return await _txIndexerDbContext.Transactions.SingleOrDefaultAsync(t => t.Hash == transactionHash, ct);
        }

        public async Task<bool> UpdateTransactionStatusAsync(Transaction transaction, CancellationToken ct)
        {
            var existingTransaction = await GetTransactionByHashAsync(transaction.Hash, ct);
            if (existingTransaction is null)
            {
                return false;
            }

            _txIndexerDbContext.Transactions.Entry(existingTransaction).Property(t => t.Status).CurrentValue = transaction.Status;
            await _txIndexerDbContext.SaveChangesAsync(ct);
            return true;
        }
    }
}
