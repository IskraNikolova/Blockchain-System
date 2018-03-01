namespace BlockExplorer.Services.Models
{
    using BlockExplorer.Services.Interfaces;
    using Newtonsoft.Json;
    using System.IO;
    using System.Net;
    
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
    }
}
