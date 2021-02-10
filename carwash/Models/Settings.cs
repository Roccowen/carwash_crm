using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace carwash.Models
{
    public class Settings
    {
        [JsonPropertyName("user_services")]
        public List<Service> UserServices { get; set; }
        public Settings()
        {

        }
    }
}
