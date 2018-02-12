namespace Miner
{
    using System;
    using System.Diagnostics;
    using Interfaces;

    public class Miner
    {
        public static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            TimeSpan blockTime = new TimeSpan(0, 0, 10);
            IEngine engine = new Engine();

            while (true)
            {
                timer.Start();
                engine.Run(timer, blockTime);
            }
        }
    }
}
