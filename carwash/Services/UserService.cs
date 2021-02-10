using carwash.Models;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace carwash.Services
{
    public static class UserService
    {
        private static bool _public = AppData.ClientServicePublic;
        public static (HttpStatusCode Status, string Token) Authorization(string Phone, string Password)
        {
            if (_public)
                return AuthorizationPublic(Phone, Password);
            else
                return AuthorizationDebug(Phone, Password);
        }
        public static (HttpStatusCode Status, string Token) Registration(string Phone, string Password, string CPassword, string Name, string Email="")
        {
            if (_public)
                return RegistrationPublic(Phone, Password, CPassword, Name, Email);
            else
                return RegistrationDebug(Phone, Password, CPassword, Name, Email);
        }
        public static (HttpStatusCode Status, User User) GetCurrentUser(string token)
        {
            if (_public)
                return GetCurrentUserPublic(token);
            else
                return GetCurrentUserDebug(token);
        }
        private static (HttpStatusCode Status, string Token) RegistrationPublic(string Phone, string Password, string CPassword, string Name = "", string Email = "")
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
        private static (HttpStatusCode Status, string Token) RegistrationDebug(string Phone, string Password, string CPassword, string Name = "", string Email = "")
        {
            return (HttpStatusCode.OK, "curent_token");
        }
        private static (HttpStatusCode Status, User User) GetCurrentUserPublic(string token)
        {
            var request = new RestRequest(@"/api/auth/user", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<User>(response.Content));
            else
                return (response.StatusCode, null);
        }
        private static (HttpStatusCode Status, User User) GetCurrentUserDebug(string token)
        {
            return (HttpStatusCode.OK, new User
            {
                Name = "Пользователь",
                Email = "debuguseremail@mail.com",
                Id = 0,
                MainUserId = "99",
                Phone = "+77777777777",
                Settings = new Settings()
                {
                    UserServices = new List<Service>()
                    {
                        new Service
                        {
                            Id = 0,
                            Title = "Стрижка",
                            Price = 3000,
                            DurationInMinuts = 60
                        },
                        new Service
                        {
                            Id = 1,
                            Title = "Подстричь концы",
                            Price = 1300,
                            DurationInMinuts = 15
                        },
                        new Service
                        {
                            Id = 2,
                            Title = "Налысо",
                            Price = 1000,
                            DurationInMinuts = 10
                        }
                    }
                }
                //"{\"user_services\":[{\"id\":0,\"title\":\"Стрижка\",\"duration_minuts\":60,\"price\":3000},{\"id\": 1,\"title\": \"Подстричь концы\",\"duration_minuts\": 30,\"price\": 2500}]}"
            });
        }
        private static (HttpStatusCode Status, string Token) AuthorizationPublic(string Phone, string Password)
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
        private static (HttpStatusCode Status, string Token) AuthorizationDebug(string Phone, string Password)
        {
            return (HttpStatusCode.OK, "curent_token");
        }
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
