namespace Wallet.Services.Interfaces
{
    using Newtonsoft.Json;
    using System;
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

        //POST
        public T Pots<T>(string resURL, object data)
        {
            var request = WebRequest.Create(resURL) as HttpWebRequest;

            request.Method = "POST";
            var jsonData = JsonConvert.SerializeObject(data);

            using (StreamWriter writer = new StreamWriter(request.GetRequestStreamAsync().Result))
            {
                writer.Write(jsonData);
            }

            try
            {
                using (var response = request.GetResponseAsync().Result)
                {
                    if (request.HaveResponse && response != null)
                    {
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
            catch (Exception exs)
            {
                WebException wex = (WebException)exs.InnerException;
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        string error;
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                             error = reader.ReadToEnd();
                            //TODO: use JSON.net to parse this string and look at the error message
                        }
                        var responseData = JsonConvert.DeserializeObject<T>(error);

                        return responseData;
                    }
                }
            }

            return default(T);
        }

        //request.Method = "POST";
        //    var jsonData = JsonConvert.SerializeObject(data);

        //    using (StreamWriter writer = new StreamWriter(request.GetRequestStreamAsync().Result))
        //    {
        //        writer.Write(jsonData);
        //    }

        //    var response = request.GetResponseAsync().Result;


        //    string responseString;
        //    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        //    {
        //        responseString = reader.ReadToEnd();
        //    }

        //    var responseData = JsonConvert.DeserializeObject<T>(responseString);

        //    return responseData;
        //}
    }
}
