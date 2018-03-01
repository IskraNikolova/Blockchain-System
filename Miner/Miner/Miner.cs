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
            TimeSpan blockTime = new TimeSpan(0, 0, 5);
            IEngine engine = Engine.Instance;

            while (true)
            {
                timer.Start();
                engine.Run(timer, blockTime);
            }
        }
    }
}
