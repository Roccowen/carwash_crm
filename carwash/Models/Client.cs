using System;
using System.Text.Json.Serialization;

namespace carwash.Models
{
    public class Client
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("car_information")]
        public string CarInformation { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

    }
}
