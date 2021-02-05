using System.Text.Json.Serialization;
using carwash.Interfaces;

namespace carwash.Models
{
    public class Worker : IPerson
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }
}
