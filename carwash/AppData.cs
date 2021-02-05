using RestSharp;
using System;

namespace carwash
{
    public partial class AppData
    {
        public static RestClient AppRestClient = new RestClient()
        {
            BaseUrl = new Uri(@"http://194.67.93.122"),
            Timeout = -1
        };
        public static int WorkersCount = 0;
        private static int _clientsCount = 0;
        public static int ClientsCount
        {
            get 
            { 
                return _clientsCount; 
            } 
            set
            {
                System.Diagnostics.Debug.WriteLine($"ClientsCount - {_clientsCount}");
                _clientsCount = value;
            }
        }
        public static int OrdersCount = 0;
        public static string TokenType = "Bearer";
        public static bool ClientServicePublic = false;
        //public static string CSSPath = "/Styles/Styles.css";
    }
}
