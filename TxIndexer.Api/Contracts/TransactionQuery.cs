using System.ComponentModel.DataAnnotations;
using TxIndexer.Api.Domain;

namespace TxIndexer.Api.Contracts
{
    public class TransactionQuery : IValidatableObject
    {
        [Range(1, int.MaxValue)] public int Page { get; init; } = 1;
        [Range(1, 100)] public int PageSize { get; init; } = 30;
        public string? Token { get; init; }
        public string? Address { get; init; }
        [Range(0, long.MaxValue)] public long? FromBlock { get; init; }
        [Range(0, long.MaxValue)] public long? ToBlock { get; init; }
        public TransactionStatus? Status { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FromBlock.HasValue && ToBlock.HasValue && FromBlock > ToBlock)
                yield return new ValidationResult
                (
                    "FromBlock must be less than or equal to ToBlock",
                    new[] {nameof(FromBlock), nameof(FromBlock) }
                );
        }
    }
}
