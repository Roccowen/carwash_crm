using System.Collections.Generic;
using System.Net;
using RestSharp;
using carwash.Models;
using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Threading;
using System.Text.Json;

namespace carwash.Services
{
    public static class OrderService
    {
        public static (HttpStatusCode Status, Order Order) NewOrder(DateTime reserveDate, 
                                                                    int clientId, 
                                                                    int workerId, 
                                                                    int price,                                            
                                                                    string token,
                                                                    string type="full",
                                                                    int status=0)
        {
            var request = new RestRequest(@"/api/order", Method.POST)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}")
                .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                .AddParameter("reserve_date", reserveDate.ToString("yyyy-MM-dd HH:mm:ss"))
                .AddParameter("client_id", clientId)
                .AddParameter("worker_id", workerId)
                .AddParameter("price", price)
                .AddParameter("type", type)
                .AddParameter("status", status);
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonConvert.DeserializeObject<Order>(response.Content, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }));
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, Order Order) GetOrderById(string orderId, string token)
        {
            var request = new RestRequest($@"/api/order/{orderId}", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonConvert.DeserializeObject<Order>(response.Content, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }));
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, List<Order> Orders) GetOrdersDebug(string token)
        {
            var request = new RestRequest(@"/api/order", Method.GET)
            {
                AlwaysMultipartFormData = true
            }
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var ordersRaw = System.Text.Json.JsonSerializer.Deserialize<List<OrderRaw>>(response.Content);
                List<Order> orders = new List<Order>();
                foreach (var orderRaw in ordersRaw)
                {
                    System.Diagnostics.Debug.WriteLine($"OrderRaw{orderRaw.Id} - {orderRaw.DateOfReservation}");
                    System.Diagnostics.Debug.WriteLine($"{DateTime.ParseExact(orderRaw.DateOfReservation, "yyyy-MM-dd HH:mm:ss", null)}");
                    orders.Add(new Order
                    {
                        ClientId = orderRaw.ClientId,
                        DateOfReservation = DateTime.ParseExact(orderRaw.DateOfReservation, "yyyy-MM-dd HH:mm:ss", null),
                        Id = orderRaw.Id,
                        Price = orderRaw.Price,
                        Status = orderRaw.Status,
                        Type = orderRaw.Type,
                        UserId = orderRaw.UserId,
                        WorkerId = orderRaw.WorkerId
                    });
                } 
                return (response.StatusCode, orders);
            }        
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, List<Order> Orders) GetOrders(string token)
        {
            var request = new RestRequest(@"/api/order", Method.GET)
            {
                AlwaysMultipartFormData = true
            }
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonConvert.DeserializeObject<List<Order>>(response.Content, new JsonSerializerSettings 
                { 
                    DateFormatString = "yyyy-MM-dd HH:mm:ss" 
                }));
            else
                return (response.StatusCode, null);
        }
        public static async Task<(HttpStatusCode Status, List<Order> Orders)> GetOrdersAsync(string token)
        {
            var request = new RestRequest(@"/api/order", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var responseTask = AppData.AppRestClient.ExecuteAsync(request);
            var response = await responseTask;
            if (response.IsSuccessful)
                return (response.StatusCode, JsonConvert.DeserializeObject<List<Order>>(response.Content, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }));
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, Order Order) ChangeOrder(string orderId,
                                                                        string token, 
                                                                        DateTime newReserveDateString, 
                                                                        int newClientId, 
                                                                        int newWorkerId, 
                                                                        int newPrice, 
                                                                        string newType, 
                                                                        string newStatus)
        {
            var request = new RestRequest($@"/api/client/{orderId}", Method.PUT)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}")
                .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                .AddParameter("reserve_date", newReserveDateString.ToString("yyyy-MM-dd HH:mm:ss"))
                .AddParameter("status", newClientId)
                .AddParameter("client_id", newWorkerId)
                .AddParameter("worker_id", newPrice)
                .AddParameter("price", newType)
                .AddParameter("type", newStatus);
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var answer = JsonConvert.DeserializeObject<ChangeOrderDataAnswer>(response.Content, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss " });
                return (response.StatusCode, answer.Order);
            }
            else
                return (response.StatusCode, null);
        }
        private class ChangeOrderDataAnswer
        {
            public Order Order { get; set; }
            public string Message { get; set; }
        }
        private class OrderRaw
        {
            [JsonPropertyName("date_of_reservation")]
            public string DateOfReservation { get; set; }
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("client_id")]
            public int ClientId { get; set; }
            [JsonPropertyName("worker_id")]
            public int WorkerId { get; set; }
            [JsonPropertyName("user_id")]
            public int UserId { get; set; }
            [JsonPropertyName("price")]
            public int Price { get; set; }
            [JsonPropertyName("type")]
            public string Type { get; set; }
            [JsonPropertyName("status")]
            public string Status { get; set; }
        }
    }
}
