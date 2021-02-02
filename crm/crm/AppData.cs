using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using RestSharp;

namespace crm
{
    public partial class AppData
    {
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
    }
}
