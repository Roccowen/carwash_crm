using System;
using System.Text.Json.Serialization;

namespace carwash.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("email_vertified_at")]
        public string MainUserId { get; set; }
        [JsonPropertyName("settings")]
        public string Settings { get; set; }
    }
}
