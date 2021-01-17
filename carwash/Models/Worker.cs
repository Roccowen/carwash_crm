using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using RestSharp;

namespace carwash.Models
{
    public class Worker
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }
}
