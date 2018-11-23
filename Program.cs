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
       static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.Start();
            Console.ReadKey();
        }
        public static void ThreadProc() {
            RunAsync().GetAwaiter().GetResult();
        }
        
        static async Task RunAsync()
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                //product = await GetRequest(url.PathAndQuery);
                //solicitar informaciòn
                Message datos = await GetRequest(url);
                
                //Enviar informaciòn
                // Update the product
                Console.WriteLine("<====POST====>...");
                //product.Price = 80;
                await PostRequest(datos);
            }   
            catch (Exception e)
            {
                DisplayException(e); 
                throw; 
            }finally 
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

        static async Task<Message> GetRequest(string url)
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
                   return list;
                }
            }
            catch (Exception ex) 
            { 
                DisplayException(ex); 
                throw; 
            } 
            
        }
        static async Task<Uri> PostRequest(Message mensaje)
        {
            IEnumerable<KeyValuePair<string,string> > queries = new List<KeyValuePair<string,string>>()
            {
                new KeyValuePair<string, string>("query1","jama2"),
                new KeyValuePair<string, string>("query1","jrodas")
            };
            HttpContent q = new FormUrlEncodedContent(queries);
            
                HttpResponseMessage response = await client.PostAsync(url,q);
                
                    /* using(HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();
                        HttpContentHeaders headers = content.Headers;
                        Console.WriteLine(mycontent);
                    }*/
                    response.EnsureSuccessStatusCode();
                    return response.Headers.Location;
                
            
        }
    }
}
