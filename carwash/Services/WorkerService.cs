using System;
using System.Collections.Generic;
using System.Text;
using carwash.Models;
using RestSharp;
using System.Text.Json;
using System.Threading.Tasks;

namespace carwash.Services
{
    public static class WorkerService
    {
        public static List<Worker> GetWorkers(string token)
        {
            var request = new RestRequest(@"/api/workers", Method.GET)
            {
                AlwaysMultipartFormData = true
            };
            request.AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var worker = JsonSerializer.Deserialize<List<Worker>>(response.Content);
                return worker;
            }
            else
                return null;
        }
        public static async Task<List<Worker>> GetWorkersAsync(string token)
        {
            var request = new RestRequest(@"/api/workers", Method.GET)
            {
                AlwaysMultipartFormData = true
            }
            .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var responseTask = AppData.AppRestClient.ExecuteAsync(request);
            var response = await responseTask;
            if (response.IsSuccessful)
            {
                var worker = JsonSerializer.Deserialize<List<Worker>>(response.Content);
                return worker;
            }
            else
                return null;
        }
        public static Worker GetWorker(string token, int id)
        {
            AppData.AppHttpClient.DefaultRequestHeaders.Add("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppHttpClient.GetAsync($"/api/workers/{id}");
            if (response.Result.IsSuccessStatusCode)
            {
                var worker = JsonSerializer.Deserialize<Worker>(response.Result.Content.ReadAsStringAsync().Result);
                AppData.AppHttpClient.DefaultRequestHeaders.Remove("Authorization");
                return worker;
            }
            else
                return null;
        }
        public static Worker NewWorker(string token, string name)
        {
            var request = new RestRequest(@"/api/workers", Method.POST)
            {
                AlwaysMultipartFormData = true
            };
            request.AddHeader("Authorization", $"{AppData.TokenType} {token}");
            request.AddParameter("name", $"{name}");
            var response = AppData.AppRestClient.ExecuteAsync(request);
            if (response.Result.IsSuccessful)
            {
                var worker = JsonSerializer.Deserialize<Worker>(response.Result.Content);
                return worker;
            }
            else
                return null;
        }
    }
}
