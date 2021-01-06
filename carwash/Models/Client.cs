using System;
using System.Text.Json.Serialization;

namespace carwash.Models
{
    public class Client
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("email_vertified_at")]
        public DateTime EmailVertifiedAt { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonPropertyName("main_user_id")]
        public string MainUserId { get; set; }
        [JsonPropertyName("settings")]
        public string Settings { get; set; }
    }
}
