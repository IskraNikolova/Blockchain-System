namespace BlockExplorer.Models
{
    using System;

    public class Transaction
    {
        public string From { get; set; }

        public string To { get; set; }

        public double Value { get; set; }

        public int Fee { get; set; }

        public DateTime DateCreated { get; set; }

        public string SenderPubKey { get; set; }

        public string[] SenderSignature { get; set; }

        public string TransactionHash { get; set; }

        public int MinedInBlockIndex { get; set; }

        public bool TransferSuccessful { get; set; }
    }
}
