using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using RestSharp;

namespace carwash
{
    public partial class AppData
    {
        public static Uri ApiRoute = new Uri(@"https://e2716a8fc3ba.ngrok.io");
        public static string csspath = "/Styles/Styles.css";
        public static HttpClient AppHttpClient = new HttpClient() 
        { 
            BaseAddress = ApiRoute 
        };
        public static RestClient AppRestClient = new RestClient() 
        { 
            BaseUrl = ApiRoute,
            Timeout = -1
        };
        public static string Token = "";
        public static string TokenType = "Bearer";
    }
}
