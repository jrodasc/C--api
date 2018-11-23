using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;

namespace apirest.Models
{
    public class Message{
        

        
        public List<Notification> Notification { get; set; }
    }

    public class Notification
    {
        public string id { get; set; }
        public string name { get; set; }
       
    }
    
 
}