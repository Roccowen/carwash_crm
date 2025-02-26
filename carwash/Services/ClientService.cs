﻿using carwash.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace carwash.Services
{
    public static class ClientService
    {
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
        public static (HttpStatusCode Status, Client Client) GetClientById(int clientId, string token)
        {
            var request = new RestRequest($@"/api/client/{clientId}", Method.GET)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<Client>(response.Content));
            else
                return (response.StatusCode, null);
        }
        public static async Task<(HttpStatusCode Status, Client Client)> GetClientByIdAsync(int clientId, string token)
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
        public static (HttpStatusCode Status, Client Client) NewClient(string name, string phone, string carInformation, string token)
        {
            if (_public)
                return NewClientPublic(name, phone, carInformation, token);
            else
                return NewClientDebug(name, phone, carInformation, token);
        }
        public static (HttpStatusCode Status, string Message) DelClient(int clientId, string token)
        {
            if (_public)
                return DelClientPublic(clientId, token);
            else
                return DelClientDebug(clientId, token);
        }
        public static (HttpStatusCode Status, Client NewClient) ChangeClientData(int clientId, string token, string newName, string newPhone, string newCarInformation)
        {
            if (_public)
                return ChangeClientDataPublic(clientId, token, newName, newPhone, newCarInformation);
            else
                return ChangeClientDataDebug(clientId, token, newName, newPhone, newCarInformation);
        }
        public static (HttpStatusCode Status, Client NewClient) ChangeClientData(Client client, string token)
        {
            if (_public)
                return ChangeClientDataPublic(client.Id, token, client.Name, client.Phone, client.CarInformation);
            else
                return ChangeClientDataDebug(client.Id, token, client.Name, client.Phone, client.CarInformation);
        }
        public static (HttpStatusCode Status, List<Client> Clients) GetClients(string token)
        {
            if (_public)
                return GetClientsPublic(token);
            else
                return GetClientsDebug();
        }
        
        private static (HttpStatusCode Status, Client Client) NewClientPublic(string name, string phone, string carInformation, string token)
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
        private static (HttpStatusCode Status, Client Client) NewClientDebug(string name, string phone, string carInformation, string token)
        {
            return (HttpStatusCode.OK, new Client()
            {
                Name = name,
                Phone = phone,
                CarInformation = carInformation,
                UserId = 0,
                Id = ClientCntInc()
            });
        }
        private static (HttpStatusCode Status, string Message) DelClientPublic(int clientId, string token)
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
        private static (HttpStatusCode Status, string Message) DelClientDebug(int clientId, string token)
        {
            return (HttpStatusCode.OK, "good");
        }
        private static (HttpStatusCode Status, Client NewClient) ChangeClientDataPublic(int clientId, string token, string newName, string newPhone, string newCarInformation)
        {
            var request = new RestRequest($@"/api/client/{clientId}", Method.PUT)
                .AddHeader("Authorization", $"{AppData.TokenType} {token}")
                .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                .AddParameter("name", newName)
                .AddParameter("phone", newPhone)
                .AddParameter("car_information", newCarInformation);
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
                return (response.StatusCode, JsonSerializer.Deserialize<ChangeClientDataAnswer>(response.Content).Client);
            else
                return (response.StatusCode, null);
        }
        private static (HttpStatusCode Status, Client NewClient) ChangeClientDataDebug(int clientId, string token, string newName, string newPhone, string newCarInformation)
        {
            return (HttpStatusCode.OK, new Client
            {
                Id = clientId,
                Name = newName,
                CarInformation = newCarInformation,
                Phone = newPhone
            });
        }
        private static (HttpStatusCode Status, List<Client> Clients) GetClientsPublic(string token)
        {
            var request = new RestRequest(@"/api/client", Method.GET)
                                .AddHeader("Authorization", $"{AppData.TokenType} {token}");
            var response = AppData.AppRestClient.Execute(request);
            if (response.IsSuccessful)
            {
                var answer = JsonSerializer.Deserialize<List<Client>>(response.Content);
                foreach (var client in answer)
                    System.Diagnostics.Debug.WriteLine($"Client-{client.Id}-{client.Name}-{client.Phone}-{client.UserId}");
                return (response.StatusCode, answer);
            }
            else
                return (response.StatusCode, null);
        }
        private static (HttpStatusCode Status, List<Client> Clients) GetClientsDebug()
        {
            var Clients = new List<Client>();
            for (int i = 1; i <= 20; i++)
            {
                Clients.Add(new Client()
                {
                    Id = i,
                    Name = _names[i],
                    CarInformation = "{\"car\":\"auto\"}",
                    Phone = $"7{_random.Next(55, 99)}{_random.Next(1111111, 9999999)}",
                    UserId = 0
                });
            }
            return (HttpStatusCode.OK, Clients);
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
        private static readonly string[] _names = {
            "Анна Артемьевна", "Никита Даниилович", "Марк Петрович", "Леонид Святославович", "Пётр Филиппович", "Никита Тимофеевич",
            "Ника Богдановна", "Мирослава Глебовна", "Александр Вадимович", "Иван Никитич", "Мария Ивановна", "Виктория Егоровна",
            "Григорий Викторович", "Алексей Романович", "София Алексеевна", "Егор Владимирович", "Ева Ивановна", "Матвей Германович",
            "Артём Тимурович", "София Максимовна", "Роман Иванович", "Андрей Степанович", "Варвара Константиновна", "Мирон Константинович",
            "Варвара Викторовна", "Илья Дмитриевич", "Адам Артёмович", "Григорий Андреевич", "Ника Андреевна", "Дарья Ивановна",
            "Елизавета Павловна", "Степан Егорович", "Ксения Ильинична", "Василиса Александровна", "София Артёмовна", "Алиса Максимовна",
            "Семён Макарович", "Александра Максимовна", "Диана Артёмовна", "Мария Серафимовна" };
        private static Random _random = new Random();
        private static int ClientCntInc(int i = 1) => AppData.ClientsCount += i;
        private static readonly bool _public = AppData.ClientServicePublic;
    }
}
