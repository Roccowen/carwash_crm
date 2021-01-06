using carwash.Models;
using System;
using System.Collections.Generic;
using RestSharp;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;

namespace carwash.Services
{
    public static class UserService
    {
        public static async Task<(HttpStatusCode Status, RegistrationAnswer Answer)> RegistrationAsync(string Phone, string Password, string CPassword, string Name = null, string Email = null)
        {
            var request = new RestRequest(@"/api/register", Method.POST)
                .AddParameter("phone", Phone)
                .AddParameter("password", Password)
                .AddParameter("c_password", CPassword)
                .AddParameter("name", Name)
                .AddParameter("email", Email);
            var responseTask = AppData.AppRestClient.ExecuteAsync(request);
            var response = await responseTask;
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<RegistrationAnswer>(response.Content);
                return (response.StatusCode, answer);
            }
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, RegistrationAnswer Answer) Registration(string Phone, string Password, string CPassword, string Name = null, string Email = null)
        {
            var request = new RestRequest(@"/api/register", Method.POST)
                .AddParameter("phone", Phone)
                .AddParameter("password", Password)
                .AddParameter("c_password", CPassword)
                .AddParameter("name", Name)
                .AddParameter("email", Email);
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<RegistrationAnswer>(response.Content);
                return (response.StatusCode, answer);
            }
            else
                return (response.StatusCode, null);
        }
        public static async Task<(HttpStatusCode Status, RegistrationAnswer Answer)> AuthorizationAsync(string Phone, string Password)
        {
            var request = new RestRequest(@"/api/login", Method.POST)
                .AddParameter("phone", Phone)
                .AddParameter("password", Password);
            var responseTask = AppData.AppRestClient.ExecuteAsync(request);
            var response = await responseTask;
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<RegistrationAnswer>(response.Content);
                return (response.StatusCode, answer);
            }
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, RegistrationAnswer Answer) Authorization(string Phone, string Password)
        {
            var request = new RestRequest(@"/api/login", Method.POST)
                .AddParameter("phone", Phone)
                .AddParameter("password", Password);
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<RegistrationAnswer>(response.Content);
                return (response.StatusCode, answer);
            }
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, User User) GetUser(string token)
        {
            var request = new RestRequest(@"/api/auth/user", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var user = JsonSerializer.Deserialize<User>(response.Content);
                return (response.StatusCode, user);
            }
            else
                return (response.StatusCode, null);
        }
    }
}
