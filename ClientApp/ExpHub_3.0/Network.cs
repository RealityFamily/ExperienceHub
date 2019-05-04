using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ExperenceHubApp
{
    public class Network
    {
        public static (string answer, HttpResponseMessage response) ResponseAwaiter(HttpContent content, string Content_Type, string uri, HttpMethod method)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();

            request.Method = method;
            request.RequestUri = new Uri(uri);
            if (method == HttpMethod.Get)
            {
                request.Headers.Add("Accept", Content_Type);
            }
            else
            {
                request.Content = content;
                request.Content.Headers.ContentType = new MediaTypeHeaderValue(Content_Type);
            }
            HttpResponseMessage response = client.SendAsync(request).GetAwaiter().GetResult();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                string json = responseContent.ReadAsStringAsync().GetAwaiter().GetResult();
                return (json, null);
            }
            return (null, response);
        }

        public static async Task<(string answer, HttpResponseMessage response)> Response(HttpContent content, string Content_Type, string uri, HttpMethod method)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();

            request.Method = method;
            request.RequestUri = new Uri(uri);
            if (method == HttpMethod.Get)
            {
                request.Headers.Add("Accept", Content_Type);
            }
            else
            {
                request.Content = content;
                request.Content.Headers.ContentType = new MediaTypeHeaderValue(Content_Type);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                string json = await responseContent.ReadAsStringAsync();
                return (json, null);
            }
            return (null, response);
        }

        
        public static string Serialize<T>(T input)
        {
            string output = JsonConvert.SerializeObject(input);
            return output;
        }

        public static T Deserialize<T>(string input)
        {
            if (input != null) {
                T output = JsonConvert.DeserializeObject<T>(input);
                return output;
            } else
            {
                return default(T);
            }
        }
    }
}
