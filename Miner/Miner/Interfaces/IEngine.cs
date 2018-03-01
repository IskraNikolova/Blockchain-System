using System;
using System.Diagnostics;

namespace Miner.Interfaces
{
    interface IEngine
    {
        void Run(Stopwatch timer, TimeSpan blockTime);
    }
}
