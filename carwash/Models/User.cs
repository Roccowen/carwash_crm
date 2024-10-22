﻿using System.Text.Json.Serialization;
using carwash.Interfaces;

namespace carwash.Models
{
    public class User : IPerson
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
        public Settings Settings { get; set; }
    }
}
