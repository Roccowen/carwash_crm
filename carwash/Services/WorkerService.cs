using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using RestSharp;
using carwash.Models;
using System.Text.Json.Serialization;
using System;

namespace carwash.Services
{
    public static class WorkerService
    {
        public static (HttpStatusCode Status, List<Worker> Workers) GetWorkers(string token)
        {
            var request = new RestRequest(@"/api/user/workers", Method.GET)
            {
                AlwaysMultipartFormData = true
            }
            .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var workersRaw = JsonSerializer.Deserialize<List<WorkerRaw>>(response.Content);
                List<Worker> workers = new List<Worker>();
                foreach (var workerRaw in workersRaw)
                    workers.Add(new Worker
                    {
                        Id = workerRaw.Id,
                        Name = workerRaw.Name,
                        UserId = Convert.ToInt32(workerRaw.UserId)
                    });
                return (response.StatusCode, workers);
            }               
            else
                return (response.StatusCode, null);
        }
        public static async Task<(HttpStatusCode Status, List<Worker> Workers)> GetWorkersAsync(string token)
        {
            var request = new RestRequest(@"/api/user/workers", Method.GET)
            {
                AlwaysMultipartFormData = true
            }
            .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var responseTask = AppData.AppRestClient.ExecuteAsync(request);
            var response = await responseTask;
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<List<Worker>>(response.Content));
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, Worker Worker) GetWorkerById(int workerId, string token)
        {
            var request = new RestRequest($@"/api/workers/{workerId}", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<Worker>(response.Content));
            else
                return (response.StatusCode, null);
        }
        public static async Task<(HttpStatusCode Status, Worker Worker)> GetWorkerByIdAsync(int workerId, string token)
        {
            var request = new RestRequest($@"/api/workers/{workerId}", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var responseTask = AppData.AppRestClient.ExecuteAsync(request);
            var response = await responseTask;
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<Worker>(response.Content));
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, Worker Worker) NewWorker(string token, string name)
        {
            var request = new RestRequest(@"/api/workers", Method.POST)
            {
                AlwaysMultipartFormData = true
            }
            .AddHeader("Authorization", $"{AppData.TokenType} {token}")
            .AddParameter("name", $"{name}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<Worker>(response.Content));
            else
                return (response.StatusCode, null);
        }
        private class WorkerRaw
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("user_id")]
            public string UserId { get; set; }
        }
    }
}
