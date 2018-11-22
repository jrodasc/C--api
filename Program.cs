using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using apirest.Models;

namespace apirest
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetRequest("http://services.fasten.com.mx/api/contenido");
            //PostRequest("http://ptsv2.com/t/m9v0g-1542909408/post");
            RunAsync("http://services.fasten.com.mx/").Wait();
            Console.ReadKey();
        }

        static async Task RunAsync(string url)
        {
            using (var contenido = new HttpClient())
            {
                contenido.BaseAddress = new Uri(url);
                contenido.DefaultRequestHeaders.Accept.Clear();
                contenido.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Console.WriteLine("GET");
                HttpResponseMessage response = await contenido.GetAsync("api/contenido");
                if(response.IsSuccessStatusCode)
                {
                    //Contenido list = await response.Content.ReadAsAsync<Taller>();
                    string data = await response.Content.ReadAsStringAsync();
                    

                    //Console.WriteLine("{0}\t{1}\t", list);
                }
            }
        }

        async static void GetRequest(string url)
        {
            using(HttpClient client = new HttpClient())
            {
                using(HttpResponseMessage response = await client.GetAsync(url))
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
