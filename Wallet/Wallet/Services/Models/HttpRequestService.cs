namespace Wallet.Services.Interfaces
{
    using Newtonsoft.Json;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    public class HttpRequestService : IHttpRequestService
    {
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

        public T Pots<T>(string resURL, object data)
        {
            var request = WebRequest.Create(resURL) as HttpWebRequest;

            request.Method = "POST";
            var jsonData = JsonConvert.SerializeObject(data);

            using (StreamWriter writer = new StreamWriter(request.GetRequestStreamAsync().Result))
            {
                writer.Write(jsonData);
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
