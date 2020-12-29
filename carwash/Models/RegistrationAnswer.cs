using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace carwash.Models
{
    public class RegistrationAnswer
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("data")]
        public Dictionary<string, string> Data { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
