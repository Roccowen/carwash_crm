using carwash.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace carwash.Services
{
    public static class WorkerService
    {
        private static bool _public = AppData.ClientServicePublic;
        private static int WorkersCntInc(int i = 1) => AppData.WorkersCount += i;
        private static int WorkersCntGet() => AppData.WorkersCount;
        public static (HttpStatusCode Status, Worker Worker) NewWorker(string token, string name)
        {
            if (_public)
                return NewWorkerPublic(token, name);
            else
                return NewWorkerDebug(token, name);
        }
        public static (HttpStatusCode Status, List<Worker> Workers) GetWorkers(string token)
        {
            if (_public)
                return GetWorkersPublic(token);
            else
                return GetWorkersDebug(token);
        }
        private static (HttpStatusCode Status, List<Worker> Workers) GetWorkersPublic(string token)
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
        private static (HttpStatusCode Status, List<Worker> Workers) GetWorkersDebug(string token)
        {
            var Workers = new List<Worker>();
            for (int i = 0; i < _random.Next(1, 10); i++)
            {
                Workers.Add(new Worker()
                {
                    Id = WorkersCntInc(),
                    Name = _names[_random.Next(0, 10)],
                    UserId = 0
                });
            }
            return (HttpStatusCode.OK, Workers);
        }
        private static (HttpStatusCode Status, Worker Worker) NewWorkerPublic(string token, string name)
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
        private static (HttpStatusCode Status, Worker Worker) NewWorkerDebug(string token, string name)
        {
            return (HttpStatusCode.OK, new Worker
            {
                Name = name,
                Id = WorkersCntInc(),
                UserId = 0
            });
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

        private class WorkerRaw
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("user_id")]
            public string UserId { get; set; }
        }
        private static string[] _names = {
            "Никита", "Марк", "Леня", "Пётр", "Никита", "Саня", "Ваня",
            "Гриша", "Алексей", "Егор", "Матвей", "Артём"};
        private static Random _random = new Random();
    }
}
