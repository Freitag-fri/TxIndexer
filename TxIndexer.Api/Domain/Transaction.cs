using System;
using System.Net.NetworkInformation;
using System.Transactions;

namespace TxIndexer.Domain
{
    public enum TransactionStatus
    {
        Pending = 0,
        Confirmed = 1,
        Failed = 2,
    }

    public sealed class Transaction : IEquatable<Transaction>
	{
        public string Hash { get; }
		public long BlockNumber { get; }
        public string From { get; }
        public string To { get; }
        public decimal Amount { get; }
        public string Token { get; }
        public long Timestamp { get; }
        public TransactionStatus Status { get; private set; }


        public Transaction(string hash, long blockNumber, string from, string to, decimal amount, string token, long timestamp, TransactionStatus status)
		{
            Hash = hash;
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
