using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Wallet.HttpService
{
    public static class PostRequest
    {
        //POST
        public static T Pots<T>(string resURL, byte[] data1)
        {
            var request = (HttpWebRequest)WebRequest.Create(resURL);

            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;
   
            using (var dataStream = request.GetRequestStream())
            {
                dataStream.Write(data1, 0, data1.Length);
            }

            var response = request.GetResponseAsync().Result;


            string responseString;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }

            var responseData = JsonConvert.DeserializeObject<T>(responseString);

            return responseData;
        }
    }
}
