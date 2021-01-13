using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using RestSharp;

namespace carwash
{
    public partial class AppData
    {
        public static Uri ApiRoute = new Uri(@"http://194.67.93.122");
        public static RestClient AppRestClient = new RestClient()
        {
            BaseUrl = ApiRoute,
            Timeout = -1
        };
        public static string TokenType = "Bearer";
        public static string CSSPath = "/Styles/Styles.css";
    }
}
