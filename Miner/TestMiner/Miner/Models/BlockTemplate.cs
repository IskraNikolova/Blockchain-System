namespace Miner.Models
{
    using Interfaces;

    public class BlockTemplate : IBlockTemplate
    {
        public int Index { get; set; }

        public double ExpectedReward { get; set; }

        public string BlockDataHash { get; set; }

        public int Difficulty { get; set; }
    }
}
