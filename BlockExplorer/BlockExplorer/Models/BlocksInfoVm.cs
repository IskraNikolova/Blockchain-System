namespace BlockExplorer.Models
{
    public class BlocksInfoVm
    {
        public string About { get; set; }

        public string Name { get; set; }

        public string[] Peers { get; set; }

        public int Difficulty { get; set; }

        public Block[] Blocks { get; set; }

        public Transaction[] ConfirmedTransactions { get; set; }

        public Transaction[] PendingTransactions { get; set; }
    }
}
