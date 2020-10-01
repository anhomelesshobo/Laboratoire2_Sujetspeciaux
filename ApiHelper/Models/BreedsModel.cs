using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiHelper.Models
{
    public class BreedsModel
    {
        [JsonProperty("message")]
        public Dictionary<string, List<string>> Breeds { get; set; }
    }
}
