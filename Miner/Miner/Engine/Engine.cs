﻿namespace Miner
{
    using System;
    using System.Diagnostics;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Interfaces;
    using Models;

    public sealed class Engine : IEngine
    {
        private static Engine instance;

        public static Engine Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Engine();
                }
                return instance;
            }
        }

        public void Run(Stopwatch timer, TimeSpan blockTime)
        {
            //Create GET request 
            string responseFromNode = Util.CreateGetRequestToNode();
           
            //Write info for starting work
            BlockTemplate blockTemplate = JsonConvert.DeserializeObject<BlockTemplate>(responseFromNode);
            string hash = blockTemplate.BlockDataHash;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nStart new task:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Number: {0}  ", blockTemplate.Index);
            Console.Write("reward: {0}  ", blockTemplate.ExpectedReward);
            Console.Write("hash: {0}... ", hash.Substring(0, 12));
            Console.Write("difficulty: {0}\n", blockTemplate.Difficulty);

            long nonce = 0;
            string timestamp = DateTime.UtcNow.ToString("o");
            string precomputedData = blockTemplate.BlockDataHash;
            bool blockFound = false;
            while (!blockFound && nonce < long.MaxValue)
            {
                string data = precomputedData + timestamp + nonce;
                string blockHash = Hash.GetHashSha256(data);

                //PoW - Sums up the first number of ASCII table characters 
                //and checks the sum if they match so they are zeros.
                int sumOfFirstSymbols = 0;
                for (int i = 0; i < blockTemplate.Difficulty; i++)
                {
                    sumOfFirstSymbols += blockHash[i];
                }

                int expectedSum = 48 * blockTemplate.Difficulty;
                if (sumOfFirstSymbols == expectedSum)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("--------------------------------------------------------");
                    Console.WriteLine($"Found Block Hash: {blockHash}\n");
                    Console.WriteLine("--------------------------------------------------------");

                    JObject dataObject = JObject.FromObject(new
                    {
                        nonce = nonce,
                        dateCreated = timestamp,
                        blockHash = blockHash
                    });

                    //POST to Node new candidate block
                    Util.CreatePostRequestToNode(dataObject);

                    blockFound = true;
                    break;
                }

                // get new timestamp on every 100000 iterations
                if (nonce % 100000 == 0)
                {
                    timestamp = DateTime.UtcNow.ToString("o");
                }

                nonce++;

                if (blockTime < timer.Elapsed)
                {
                    timer.Reset();
                    break;
                }
            }
        }
    }
}
