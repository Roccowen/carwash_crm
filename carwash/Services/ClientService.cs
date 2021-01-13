using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using RestSharp;
using carwash.Models;
using System.Threading.Tasks;
using System.Threading;

namespace carwash.Services
{
    public static class ClientService
    {
        public static (HttpStatusCode Status, Client Client) NewClient(string name, string phone, string carInformation, string token)
        {
            var request = new RestRequest(@"/api/client", Method.POST)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}")
                .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                .AddParameter("name", name)
                .AddParameter("phone", phone)
                .AddParameter("car_information", carInformation);
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<NewClientAnswer>(response.Content);
                return (response.StatusCode, answer.Client);
            }
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, string Message) DelClient(string clientId, string token)
        {
            var request = new RestRequest($@"/api/client/{clientId}", Method.DELETE)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<DelClientAnswer>(response.Content);
                return (response.StatusCode, answer.Message);
            }
            else
                return (response.StatusCode, "");
        }
        public static (HttpStatusCode Status, Client NewClient) ChangeClientData(string clientId, 
                                                                                    string token, 
                                                                                    string newName = "", 
                                                                                    string newPhone = "", 
                                                                                    string newCarInformation = "")
        {
            var request = new RestRequest($@"/api/client/{clientId}", Method.PUT)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}")
                .AddHeader("Content-Type", "application/x-www-form-urlencoded");
            if (newName != "")
                request.AddParameter("name", newName);
            if (newPhone != "")
                request.AddParameter("phone", newPhone);
            if (newCarInformation != "")
                request.AddParameter("car_information", newCarInformation);
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<ChangeClientDataAnswer>(response.Content).Client);
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, List<Client> Clients) GetClients(string token)
        {
            var request = new RestRequest(@"/api/client", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<List<Client>>(response.Content);
                return (response.StatusCode, answer);
            }
            else
                return (response.StatusCode, null);
        }
        public static async Task<(HttpStatusCode Status, List<Client> Clients)> GetClientsAsync(string token)
        {
            var request = new RestRequest(@"/api/client", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var responseTask = AppData.AppRestClient.ExecuteAsync(request);
            var response = await responseTask;
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<List<Client>>(response.Content);
                return (response.StatusCode, answer);
            }
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, Client Client) GetClientsById(int clientId, string token)
        {
            var request = new RestRequest($@"/api/client/{clientId}", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<Client>(response.Content));
            else
                return (response.StatusCode, null);
        }
        public static async Task<(HttpStatusCode Status, Client Client)> GetClientsByIdAsync(int clientId, string token)
        {
            var request = new RestRequest($@"/api/client/{clientId}", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var responseTask = AppData.AppRestClient.ExecuteAsync(request);
            var response = await responseTask;
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<Client>(response.Content));
            else
                return (response.StatusCode, null);
        }
        private class NewClientAnswer
        {
            [JsonPropertyName("client")]
            public Client Client { get; set; }
            [JsonPropertyName("message")]
            public string Message { get; set; }
        }
        private class ChangeClientDataAnswer
        {
            [JsonPropertyName("0")]
            public Client Client { get; set; }
            [JsonPropertyName("message")]
            public string Message { get; set; }
        }
        private class DelClientAnswer
        {
            [JsonPropertyName("message")]
            public string Message { get; set; }
        }
    }
}
