using System.Text.Json.Serialization;

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
