using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using RestSharp;
using carwash.Models;

namespace carwash.Services
{
    public static class UserService
    {
        public static async Task<(HttpStatusCode Status, string Token)> RegistrationAsync(string Phone, string Password, string CPassword, string Name = "", string Email = "")
        {
            var request = new RestRequest(@"/api/register", Method.POST)
                .AddParameter("phone", Phone)
                .AddParameter("password", Password)
                .AddParameter("c_password", CPassword);
            if (Name != "")
                request.AddParameter("name", Name);
            if (Email != "")
                request.AddParameter("email", Email);
            var responseTask = AppData.AppRestClient.ExecuteAsync(request);
            var response = await responseTask;
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<RegistrationAnswer>(response.Content);
                return (response.StatusCode, answer.Data["token"]);
            }
            else
                return (response.StatusCode, "");
        }
        public static (HttpStatusCode Status, string Token) Registration(string Phone, string Password, string CPassword, string Name = "", string Email = "")
        {
            var request = new RestRequest(@"/api/register", Method.POST)
                .AddParameter("phone", Phone)
                .AddParameter("password", Password)
                .AddParameter("c_password", CPassword);
            if (Name != "")
                request.AddParameter("name", Name);
            if (Email != "")
                request.AddParameter("email", Email);
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<RegistrationAnswer>(response.Content);
                return (response.StatusCode, answer.Data["token"]);
            }
            else
                return (response.StatusCode, "");
        }
        public static async Task<(HttpStatusCode Status, string Token)> AuthorizationAsync(string Phone, string Password)
        {
            var request = new RestRequest(@"/api/login", Method.POST)
                .AddParameter("phone", Phone)
                .AddParameter("password", Password);
            var responseTask = AppData.AppRestClient.ExecuteAsync(request);
            var response = await responseTask;
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<RegistrationAnswer>(response.Content);
                return (response.StatusCode, answer.Data["token"]);
            }
            else
                return (response.StatusCode, "");
        }
        public static (HttpStatusCode Status, string Token) Authorization(string Phone, string Password)
        {
            var request = new RestRequest(@"/api/login", Method.POST)
                .AddParameter("phone", Phone)
                .AddParameter("password", Password);
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<RegistrationAnswer>(response.Content);
                return (response.StatusCode, answer.Data["token"]);
            }
            else
                return (response.StatusCode, "");
        }
        public static (HttpStatusCode Status, User User) GetCurrentUser(string token)
        {
            var request = new RestRequest(@"/api/auth/user", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<User>(response.Content));
            else
                return (response.StatusCode, null);
        }
        private class RegistrationAnswer
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }
            [JsonPropertyName("data")]
            public Dictionary<string, string> Data { get; set; }
            [JsonPropertyName("message")]
            public string Message { get; set; }
        }
    }
}
