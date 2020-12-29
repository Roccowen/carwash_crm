using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using RestSharp;

namespace carwash.Models
{
    public class Worker
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
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
        public static List<Worker> GetWorkersSys(string token)
        {
            AppData.AppHttpClient.DefaultRequestHeaders.Add("Authorization", $"{AppData.TokenType} {AppData.Token}");
            var response = AppData.AppHttpClient.GetAsync($"/api/workers/");
            System.Diagnostics.Debug.WriteLine(response.Result);
            if (response.Result.IsSuccessStatusCode)
            {
                var workers = JsonSerializer.Deserialize<List<Worker>>(response.Result.Content.ReadAsStringAsync().Result);               
                AppData.AppHttpClient.DefaultRequestHeaders.Remove("Authorization");
                return workers;
            }
            else
            {
                AppData.AppHttpClient.DefaultRequestHeaders.Remove("Authorization");
                return null;
            }
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
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var worker = JsonSerializer.Deserialize<Worker>(response.Content);
                return worker;
            }
            else
                return null;
        }
        public static Worker NewWorkerSys(string token, string name_) 
        {
            var data = new{
                        name = name_
                    };
            AppData.AppHttpClient.DefaultRequestHeaders.Add("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppHttpClient.PostAsJsonAsync(@"/api/workers/", data);
            if (response.Result.IsSuccessStatusCode)
            {
                var worker = JsonSerializer.Deserialize<Worker>(response.Result.Content.ReadAsStringAsync().Result);
                AppData.AppHttpClient.DefaultRequestHeaders.Remove("Authorization");
                return worker;
            }
            else
                return null;
        }
    }
}
