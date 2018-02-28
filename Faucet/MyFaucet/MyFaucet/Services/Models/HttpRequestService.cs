namespace MyFaucet.Services.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Net;
    using System.Text;
    using MyFaucet.Models;
    using MyFaucet.Services.Interfaces;

    public class HttpRequestService : IHttpRequestService
    {
        public ResponseModel Post(string url, SendTransactionBody data)
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

            WebResponse response = request.GetResponse();//todo get 400
            string responseString;
            statusCode = ((HttpWebResponse)response).StatusCode;

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }

            var responseData = JsonConvert.DeserializeObject<ResponseModel>(responseString);

            return responseData;
           
        }
    }
}
