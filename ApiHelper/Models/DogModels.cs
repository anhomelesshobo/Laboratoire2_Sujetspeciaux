using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;


namespace ApiHelper.Models
{
    public class DogModels
    {
        
        public string Nom { get; set; }

        [JsonProperty("message")]
        public string source { get; set; }

      



    }
}
