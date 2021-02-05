using carwash.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace carwash.Services
{
    public static class OrderService
    {
        private static bool _public = AppData.ClientServicePublic;
        private static int OrdersCntInc(int i = 1) => AppData.OrdersCount += i;
        private static int OrdersCntGet() => AppData.OrdersCount;
        public static (HttpStatusCode Status, string message) DelOrderById(int id, string token)
        {
            if (_public)
                return DelOrderByIdPublic(id, token);
            else
                return DelOrderByIdDebug(id);
        }
        public static (HttpStatusCode Status, List<Order> Orders) GetOrders(string token)
        {
            if (_public)
                return GetOrdersPublic(token);
            else
                return GetOrdersDebug(token);
        }
        public static (HttpStatusCode Status, Order Order) NewOrder(DateTime reserveDate, int clientId, int workerId, int price, string token, string type = "full", int status = 0)
        {
            if (_public)
                return NewOrderPublic(reserveDate, clientId, workerId, price, token, type, status);
            else
                return NewOrderDebug(reserveDate, clientId, workerId, price, token, type, status);
        }
        public static (HttpStatusCode Status, Order Order) GetOrderById(int orderId, string token)
        {
            if (_public)
                return GetOrderByIdPublic(orderId, token);
            else
                return GetOrderByIdDebug(orderId, token);        
        }
        private static (HttpStatusCode Status, List<Order> Orders) GetOrdersPublic(string token)
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
                    Debug.WriteLine($"OrderRaw{orderRaw.Id} - {orderRaw.DateOfReservation}");
                    Debug.WriteLine($"{DateTime.ParseExact(orderRaw.DateOfReservation, "yyyy-MM-dd HH:mm:ss", null)}");
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
        private static (HttpStatusCode Status, List<Order> Orders) GetOrdersDebug(string token)
        {
            var Orders = new List<Order>();
            for (int i = 0; i < _random.Next(15, 40); i++)
            {
                Orders.Add(new Order()
                {
                    Id = OrdersCntInc(),
                    ClientId = _random.Next(1, AppData.ClientsCount),
                    WorkerId = _random.Next(1, AppData.WorkersCount),
                    Price = _random.Next(1, 50) * 100,
                    Type = "full",
                    Status = "0",
                    DateOfReservation = GetRandomDay(),
                    UserId = 0
                });
            }
            return (HttpStatusCode.OK, Orders);
        }
        private static (HttpStatusCode Status, Order Order) NewOrderPublic(DateTime reserveDate, int clientId, int workerId, int price, string token, string type = "full", int status = 0)
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
            {
                var orderRaw = System.Text.Json.JsonSerializer.Deserialize<NewOrderRaw>(response.Content);
                Debug.WriteLine($"OrderRaw{orderRaw.Id} - {orderRaw.DateOfReservation}");
                Debug.WriteLine($"{DateTime.ParseExact(orderRaw.DateOfReservation, "yyyy-MM-dd HH:mm:ss", null)}");
                var order = new Order
                {
                    ClientId = Convert.ToInt32(orderRaw.ClientId),
                    DateOfReservation = DateTime.ParseExact(orderRaw.DateOfReservation, "yyyy-MM-dd HH:mm:ss", null),
                    Id = orderRaw.Id,
                    Price = Convert.ToInt32(orderRaw.Price),
                    Status = orderRaw.Status,
                    Type = orderRaw.Type,
                    UserId = orderRaw.UserId,
                    WorkerId = Convert.ToInt32(orderRaw.WorkerId)
                };
                return (response.StatusCode, order);
            }
            else
                return (response.StatusCode, null);
        }       
        private static (HttpStatusCode Status, Order Order) NewOrderDebug(DateTime reserveDate, int clientId, int workerId, int price, string token, string type = "full", int status = 0)
        {
            return (HttpStatusCode.OK, new Order
            {
                Id = OrdersCntInc(),
                ClientId = clientId,
                WorkerId = workerId,
                Price = price,
                Type = type,
                Status = status.ToString(),
                UserId = 0,
                DateOfReservation = reserveDate
            });
        }
        private static (HttpStatusCode Status, string message) DelOrderByIdPublic(int id, string token)
        {
            throw new MissingMethodException();
        }
        private static (HttpStatusCode Status, string message) DelOrderByIdDebug(int id)
        {
            return (HttpStatusCode.OK, "good");
        }
        private static (HttpStatusCode Status, Order order) GetOrderByIdPublic(int orderId, string token)
        {
            var request = new RestRequest($@"/api/order/{orderId}", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonConvert.DeserializeObject<Order>(response.Content, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }));
            else
                return (response.StatusCode, null);
        }
        private static (HttpStatusCode Status, Order order) GetOrderByIdDebug(int orderId, string token)
        {
            if (orderId > OrdersCntGet())
                return (HttpStatusCode.OK, DBService.GetOrderById(orderId));
            else
                return (HttpStatusCode.InternalServerError, null);
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
        private class NewOrderRaw
        {
            [JsonPropertyName("date_of_reservation")]
            public string DateOfReservation { get; set; }
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("client_id")]
            public string ClientId { get; set; }
            [JsonPropertyName("worker_id")]
            public string WorkerId { get; set; }
            [JsonPropertyName("user_id")]
            public int UserId { get; set; }
            [JsonPropertyName("price")]
            public string Price { get; set; }
            [JsonPropertyName("type")]
            public string Type { get; set; }
            [JsonPropertyName("status")]
            public string Status { get; set; }
        }
        private static (HttpStatusCode Status, List<Order> Orders) GetOrdersNW(string token)
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
        public static (HttpStatusCode Status, Order Order) ChangeOrder(int orderId, string token, DateTime newReserveDateString, int newClientId, int newWorkerId, int newPrice, string newType, string newStatus)
        {
            if (_public)
                return ChangeOrderPublic(orderId, token, newReserveDateString, newClientId, newWorkerId, newPrice, newType, newStatus);
            else
                return ChangeOrderDebug(orderId, token, newReserveDateString, newClientId, newWorkerId, newPrice, newType, newStatus);
        }
        public static (HttpStatusCode Status, Order Order) ChangeOrder(Order order, string token)
        {
            if (_public)
                return ChangeOrderPublic(order.Id, token, order.DateOfReservation, order.ClientId, order.WorkerId, order.Price, order.Type, order.Status);
            else
                return ChangeOrderDebug(order.Id, token, order.DateOfReservation, order.ClientId, order.WorkerId, order.Price, order.Type, order.Status);
        }
        private static (HttpStatusCode Status, Order Order) ChangeOrderDebug(int orderId, string token, DateTime newReserveDateString, int newClientId, int newWorkerId, int newPrice, string newType, string newStatus)
        {
            return (HttpStatusCode.OK, new Order
            {
                Id = orderId,
                UserId = 0,
                ClientId = newClientId,
                WorkerId = newWorkerId,
                DateOfReservation = newReserveDateString,
                Price = newPrice,
                Type = newType,
                Status = newStatus
            });
        }
        private static (HttpStatusCode Status, Order Order) ChangeOrderPublic(int orderId, string token, DateTime newReserveDateString, int newClientId, int newWorkerId, int newPrice, string newType, string newStatus)
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
        private static Random _random = new Random();
        private static DateTime GetRandomDay() => DateTime.Today.AddDays(_random.Next(-3, 3)).AddHours(_random.Next(9, 19));
        private static (HttpStatusCode Status, Order Order) NewOrderNW(DateTime reserveDate, int clientId, int workerId, int price, string token, string type = "full", int status = 0)
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
            {
                var createdOrder = JsonConvert.DeserializeObject<Order>(response.Content, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                Debug.WriteLine($"@NewOrder {createdOrder.Id}-{createdOrder.Client}-{createdOrder.Price}");
                return (response.StatusCode, createdOrder);
            }
            else
                return (response.StatusCode, null);
        }
    }
}
