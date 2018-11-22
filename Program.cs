using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace apirest
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetRequest("http://www.google.com.mx");
            PostRequest("http://ptsv2.com/t/m9v0g-1542909408/post");
            Console.ReadKey();
        }

        async static void GetRequest(string url)
        {
            using(HttpClient client = new HttpClient())
            {
                using(HttpResponseMessage response = await client.GetAsync(url))
                {
                    using(HttpContent content = response.Content)
                    {
                        //string mycontent = await content.ReadAsStringAsync();
                        HttpContentHeaders headers = content.Headers;
                        Console.WriteLine(headers);
                    }
                }
            }
        }
        async static void PostRequest(string url)
        {
            IEnumerable<KeyValuePair<string,string> > queries = new List<KeyValuePair<string,string>>()
            {
                new KeyValuePair<string, string>("query1","jama2"),
                new KeyValuePair<string, string>("query1","jrodas")
            };
            HttpContent q = new FormUrlEncodedContent(queries);
            using(HttpClient client = new HttpClient())
            {
                using(HttpResponseMessage response = await client.PostAsync(url,q))
                {
                    using(HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();
                        HttpContentHeaders headers = content.Headers;
                        Console.WriteLine(mycontent);
                    }
                }
            }
        }
    }
}
