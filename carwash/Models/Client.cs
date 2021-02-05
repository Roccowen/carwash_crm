using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using carwash.Interfaces;

namespace carwash.Models
{
    public class Client : IPerson
    {
        [Key]
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
