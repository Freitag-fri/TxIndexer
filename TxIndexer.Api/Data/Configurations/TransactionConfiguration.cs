using Microsoft.EntityFrameworkCore;
using TxIndexer.Api.Domain;

namespace TxIndexer.Api.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(t => t.Hash)
                .IsRequired()
                .HasMaxLength(66);

            builder.HasIndex(t => t.Hash)
                .IsUnique();

                builder.Property(t => t.From)
                    .IsRequired()
                    .HasMaxLength(42);

            builder.Property(t => t.To)
                .IsRequired()
                .HasMaxLength(42);

            builder.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(t => t.Amount)
                .HasPrecision(28, 18);
        }
    }
}
