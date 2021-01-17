using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using RestSharp;
using carwash.Models;

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
                return (response.StatusCode, JsonSerializer.Deserialize<List<Worker>>(response.Content));
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
    }
}
