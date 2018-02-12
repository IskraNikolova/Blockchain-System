namespace Miner.Interfaces
{
    interface IBlockTemplate
    {
        int Index { get; set; }

        double ExpectedReward { get; set; }

        string BlockDataHash { get; set; }

        int Difficulty { get; set; }
    }
}
