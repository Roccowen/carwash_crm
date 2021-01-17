using System;
using System.Text.Json.Serialization;

namespace carwash.Models
{
    public class Order
    {
        [JsonPropertyName("date_of_reservation")]
        public DateTime DateOfReservation { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("client_id")]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [JsonPropertyName("worker_id")]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("price")]
        public int Price { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
