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
            var request = WebRequest.Create(resURL) as HttpWebRequest;

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

            var jsonData = JsonConvert.SerializeObject(dataObject);
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(jsonData);
            }

            var response = request.GetResponse();
            string responseString;

            using(StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }

            var responseData = JsonConvert.DeserializeObject<T>(responseString);

            return responseData;
        }


        public ResponseSentTransactionVm Post(string url, SendTransactionBody data)
        {
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

            byte[] blockFoundData = Encoding.UTF8.GetBytes(dataObject.ToString());
            HttpStatusCode statusCode = HttpStatusCode.RequestTimeout;

            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.Timeout = 3000;
            request.ContentType = "application/json; charset=utf-8";

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(blockFoundData, 0, blockFoundData.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            string responseString;
            statusCode = ((HttpWebResponse)response).StatusCode;

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }

            var responseData = JsonConvert.DeserializeObject<ResponseSentTransactionVm>(responseString);

            return responseData;
           
        }
    }
}
