using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using RestSharp;
using carwash.Models;
using System;

namespace carwash.Services
{
    public static class OrderService
    {
        public static (HttpStatusCode Status, Order Order) NewOrder(DateTime reserveDate, 
                                                                    string clientId, 
                                                                    string workerId, 
                                                                    string price, 
                                                                    string type, 
                                                                    string status, 
                                                                    string token)
        {
            var request = new RestRequest(@"/api/order", Method.POST)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}")
                .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                .AddParameter("reserve_date", reserveDate.ToString())
                .AddParameter("client_id", clientId)
                .AddParameter("worker_id", workerId)
                .AddParameter("price", price)
                .AddParameter("type", type)
                .AddParameter("status", status);
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<Order>(response.Content));
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, Order Order) GetOrderById(string orderId, string token)
        {
            var request = new RestRequest($@"/api/order/{orderId}", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<Order>(response.Content));
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, List<Order> Orders) GetOrders(string token)
        {
            var request = new RestRequest(@"/api/order", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<List<Order>>(response.Content));
            else
                return (response.StatusCode, null);
        }
        public static (HttpStatusCode Status, Order Order) ChangeOrder(string orderId,
                                                                        string token, 
                                                                        string newReserveDateString = "", 
                                                                        string newClientId = "", 
                                                                        string newWorkerId = "", 
                                                                        string newPrice = "", 
                                                                        string newType = "", 
                                                                        string newStatus = "")
        {
            var request = new RestRequest($@"/api/client/{orderId}", Method.PUT)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}")
                .AddHeader("Content-Type", "application/x-www-form-urlencoded");
            if (newReserveDateString != "")
                request.AddParameter("reserve_date", newReserveDateString);
            if (newClientId != "")
                request.AddParameter("status", newClientId);
            if (newWorkerId != "")
                request.AddParameter("client_id", newWorkerId);
            if (newPrice != "")
                request.AddParameter("worker_id", newPrice);
            if (newType != "")
                request.AddParameter("price", newType);
            if (newStatus != "")
                request.AddParameter("type", newStatus);
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<ChangeOrderDataAnswer>(response.Content);
                return (response.StatusCode, answer.Order);
            }
            else
                return (response.StatusCode, null);
        }
        private class ChangeOrderDataAnswer
        {
            [JsonPropertyName("0")]
            public Order Order { get; set; }
            [JsonPropertyName("message")]
            public string Message { get; set; }
        }
    }
}
