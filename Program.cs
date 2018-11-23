using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using apirest.Models;
using System.Data;
using Newtonsoft.Json.Schema;
using System.Threading;

namespace apirest
{
    class Program
    {
       static string url = "http://services.fasten.com.mx/";
        static void Main(string[] args)
        {
            //GetRequest("http://services.fasten.com.mx/api/contenido");
            //PostRequest("http://ptsv2.com/t/m9v0g-1542909408/post");
            //Creamos el delegado 
           
            RunAsync().GetAwaiter().GetResult();
            Console.ReadKey();
        }

        static async Task RunAsync()
        {
            try 
            {    Message list = null;
                using (var contenido = new HttpClient())
                {
                    contenido.BaseAddress = new Uri(url);
                    contenido.Timeout = new TimeSpan(0, 2, 0);  //2 minutes
                    contenido.DefaultRequestHeaders.Accept.Clear();
                    contenido.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    Console.WriteLine("GET");
                    HttpResponseMessage response = await contenido.GetAsync("api/contenido");
                    
                    if(response.IsSuccessStatusCode)
                    {
                        string json = @"{
                                    'Notification': [
                                        {
                                        'id': '0',
                                        'name': 'item 0'
                                        },
                                        {
                                        'id': '1',
                                        'name': 'item 1'
                                        }
                                    ]
                                    }";

                        list = JsonConvert.DeserializeObject<Message>(json);
                        foreach(var men in list.Notification.ToArray())
                        {
                            Console.WriteLine(men.name);   
                        }
                       
                    }
                   
                }
            }
            catch (Exception ex) 
            { 
                DisplayException(ex); 
                throw; 
            } 
            finally 
            { 
                Console.WriteLine("Press <Enter> to exit the program."); 
                Console.ReadLine(); 
            } 
        }

        private static void DisplayException(Exception ex) 
        { 
            Console.WriteLine("The application terminated with an error."); 
            Console.WriteLine(ex.Message); 
            while (ex.InnerException != null) 
            { 
                Console.WriteLine("\t* {0}", ex.InnerException.Message); 
                ex = ex.InnerException; 
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
