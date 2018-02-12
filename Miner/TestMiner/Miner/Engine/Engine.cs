namespace Miner
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Diagnostics;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Interfaces;
    using Models;

    public sealed class Engine : IEngine
    {
        public void Run(Stopwatch timer, TimeSpan blockTime)
        {
            WebResponse response = null;
            HttpStatusCode statusCode = HttpStatusCode.RequestTimeout;

            //Create GET request 
            response = Util.CreateGetRequestToNode(response, statusCode);

            // Read the content.
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromNode = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();

            //Write Info For Starting Work
            BlockTemplate blockTemplate = JsonConvert
                .DeserializeObject<BlockTemplate>(responseFromNode);
            Console.WriteLine("\nStart new task:");
            Console.WriteLine("Block Index: {0}", blockTemplate.Index);
            Console.WriteLine("Expected Reward: {0}", blockTemplate.ExpectedReward);
            Console.WriteLine("Block Data Hash: {0}", blockTemplate.BlockDataHash);
            Console.WriteLine("Difficulty: {0}\n", blockTemplate.Difficulty);

            bool blockFound = false;
            long nonce = 0;
            string timestamp = DateTime.UtcNow.ToString("o");
            string difficulty = new String('0', blockTemplate.Difficulty) +
                new String('9', 64 - blockTemplate.Difficulty);

            // blockHash = SHA256(BlockDataHash|Nonce|DataCreated);
            string precomputedData = blockTemplate.BlockDataHash;
            string data;
            string blockHash;

            while (!blockFound && nonce < int.MaxValue)
            {
                data = precomputedData + timestamp + nonce;
                blockHash = Util.ByteArrayToHexString(Util.Sha256(Encoding.UTF8.GetBytes(data)));
                if (string.CompareOrdinal(blockHash, difficulty) < 0)
                {
                    Console.WriteLine("!!! Block found !!!");
                    Console.WriteLine($"Block Hash: {blockHash}\n");

                    JObject dataObject = JObject.FromObject(new
                    {
                        nonce = nonce,
                        dateCreated = timestamp,
                        blockHash = blockHash
                    });

                    //POST to Node new candidate block
                    Util.CreatePostRequestToNode(statusCode, dataStream, response, dataObject);

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
