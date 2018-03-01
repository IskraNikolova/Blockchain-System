namespace BlockExplorer.Models
{
    using System;

    public class Block
    {
        public int Index { get; set; }

        public Transaction[] Transactions { get; set; }

        public int Difficulty { get; set; }

        public string PrevBlockHash { get; set; }

        public string MinedBy { get; set; }

        public string BlockDataHash { get; set; }

        public int Nonce { get; set; }

        public DateTime DateCreated { get; set; }

        public string BlockHash { get; set; }
    }
}
