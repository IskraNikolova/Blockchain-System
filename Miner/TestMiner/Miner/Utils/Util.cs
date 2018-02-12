namespace Miner
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using Utils;

    public static class Util
    {
        public static WebResponse CreateGetRequestToNode(WebResponse response, HttpStatusCode statusCode)
        {
            statusCode = HttpStatusCode.RequestTimeout;
            do
            {
                try
                {
                    statusCode = HttpStatusCode.RequestTimeout;

                    // Create a request to Node   
                    WebRequest request = WebRequest.Create(Constants.NodeURL + "/mining/get-block/" + Constants.MinerAddress);
                    request.Method = "GET";
                    request.Timeout = 3000;
                    request.ContentType = "application/json; charset=utf-8";

                    response = request.GetResponse();
                    statusCode = ((HttpWebResponse)response).StatusCode;
                }
                catch (WebException e)
                {
                    Console.WriteLine("WebException raised!");
                    Console.WriteLine("{0}\n", e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception raised!");
                    Console.WriteLine("Source : {0}", e.Source);
                    Console.WriteLine("Message : {0}\n", e.Message);
                }
            } while (statusCode != HttpStatusCode.OK);

            return response;
        }

        public static void CreatePostRequestToNode(
            HttpStatusCode statusCode, 
            Stream dataStream, 
            WebResponse response, 
            JObject dataObject)
        {
            byte[] blockFoundData = Encoding.UTF8.GetBytes(dataObject.ToString());
            int retries = 0;

            do
            {
                try
                {
                    statusCode = HttpStatusCode.RequestTimeout;

                    WebRequest request = WebRequest.Create(Constants.NodeURL + "/mining/submit-block/" + Constants.MinerAddress);
                    request.Method = "POST";
                    request.Timeout = 3000;
                    request.ContentType = "application/json; charset=utf-8";

                    dataStream = request.GetRequestStream();
                    dataStream.Write(blockFoundData, 0, blockFoundData.Length);
                    dataStream.Close();

                    response = request.GetResponse();
                    statusCode = ((HttpWebResponse)response).StatusCode;

                    // Display the status.
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    response.Close();
                }
                catch (WebException e)
                {
                    Console.WriteLine("WebException raised!");
                    Console.WriteLine("{0}\n", e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception raised!");
                    Console.WriteLine("Source : {0}", e.Source);
                    Console.WriteLine("Message : {0}\n", e.Message);
                }

                System.Threading.Thread.Sleep(1000);
            } while (statusCode != HttpStatusCode.OK && retries++ < 3);
        }

        public static byte[] Sha256(byte[] array)
        {
            SHA256Managed hashstring = new SHA256Managed();
            return hashstring.ComputeHash(array);
        }

        public static string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            string hexAlphabet = "0123456789ABCDEF";

            foreach (byte b in bytes)
            {
                result.Append(hexAlphabet[(int)(b >> 4)]);
                result.Append(hexAlphabet[(int)(b & 0x0F)]);
            }

            return result.ToString();
        }
    }
}
