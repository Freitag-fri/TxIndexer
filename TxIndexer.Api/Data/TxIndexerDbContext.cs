using Microsoft.EntityFrameworkCore;
using TxIndexer.Api.Domain;

namespace TxIndexer.Api.Data
{
    public class TxIndexerDbContext : DbContext
    {
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public TxIndexerDbContext(DbContextOptions<TxIndexerDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TxIndexerDbContext).Assembly);
        }
    }
}
