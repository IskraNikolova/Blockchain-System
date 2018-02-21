namespace Wallet.Services.Interfaces
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Net;
    using System.Text;
    using Wallet.Models.ViewModels;
    using System.Collections.Specialized;

    public class HttpRequestService : IHttpRequestService
    {
        //GET
        public T Get<T>(string resUrl)
        {
            var request = WebRequest.Create(resUrl) as HttpWebRequest;

            request.ContentType = "application/json";
            request.Method = "GET";
            WebResponse response = request.GetResponseAsync().Result;
            string responseString;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }

            var responseData = JsonConvert.DeserializeObject<T>(responseString);

            return responseData;
        }

        //POST
        public T Pots<T>(string resURL, SendTransactionBody data)
        {
            var request = (HttpWebRequest)WebRequest.Create(resURL);

            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;
            JObject dataObject = JObject.FromObject(new
            {
                from = data.From,
                to = data.To,
                value = data.Value,
                fee = data.Fee,
                dateCreated = data.DateCreated,
                senderPubKey = data.SenderPubKey,
                senderSignature = data.SenderSignature
            });

            byte[] data1 = Encoding.ASCII.GetBytes(dataObject.ToString());

            using (var dataStream = request.GetRequestStreamAsync().Result)
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
