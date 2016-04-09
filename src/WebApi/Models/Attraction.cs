using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Attraction
    {
        [JsonProperty(PropertyName = "attractionId")]
        public string AttractionId { get; set; }

    }
}