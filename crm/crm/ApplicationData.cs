using System;
using carwash.Models;
using carwash.Services;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using RestSharp;
using System.Collections.ObjectModel;

namespace crm
{
    public static partial class ApplicationData
    {
        public static ObservableCollection<Client> Clients { get; set; }
        public static Uri ApiRoute = new Uri(@"http://194.67.93.122");
        public static RestClient AppRestClient = new RestClient()
        {
            BaseUrl = ApiRoute,
            Timeout = -1
        };
        public static int WorkersCount = 0;
        public static int ClientsCount = 0;
        public static int OrdersCount = 0;
        public static bool ClientServicePublic = false;
        public static string TokenType = "Bearer";
        //public static string CSSPath = "/Styles/Styles.css";
        public void Initialization()
        {
            Clients = new ObservableCollection<Client>();
            foreach (var c in DBService.GetClients())
                
        }
    }
}
