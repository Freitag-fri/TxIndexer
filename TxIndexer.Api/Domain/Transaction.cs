namespace TxIndexer.Api.Domain
{
    public sealed class Transaction : IEquatable<Transaction>
	{
        public long Id { get; private set; }
        public string Hash { get; private set; }
		public long BlockNumber { get; private set; }
        public string From { get; private set; }
        public string To { get; private set; }
        public decimal Amount { get; private set; }
        public string Token { get; private set; }
        public long Timestamp { get; private set; }
        public TransactionStatus Status { get; private set; }


        public Transaction(string hash, long blockNumber, string from, string to, decimal amount, string token, long timestamp, TransactionStatus status)
		{
            Hash = hash.ToLowerInvariant();
            BlockNumber = blockNumber;
            From = from;
            To = to;
            Amount = amount;
            Token = token;
            Timestamp = timestamp;
            Status = status;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Transaction);
        }

        public bool Equals(Transaction? other)
        {
            return other is not null &&
                Hash == other.Hash &&
                BlockNumber == other.BlockNumber &&
                From == other.From &&
                To == other.To &&
                Amount == other.Amount;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Hash, BlockNumber, From, To, Amount);
        }

        public void MarkConfirmed()
        {
            if(!IsTransactionPending())
            {
                throw new InvalidOperationException("Current transaction status is not Pending");
            }

            this.Status = TransactionStatus.Confirmed;
        }

        public void MarkFailed()
        {
            if (!IsTransactionPending())
            {
                throw new InvalidOperationException("Current transaction status is not Pending");
            }

            this.Status = TransactionStatus.Failed;
        }

        private bool IsTransactionPending()
        {
            return this.Status == TransactionStatus.Pending;    
        }
    }
}
