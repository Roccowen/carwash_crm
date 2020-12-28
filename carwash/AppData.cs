using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace carwash
{
    public static class AppData
    {
        public static HttpClient AppHttpClient = new HttpClient();
        public static Uri ApiRoute = new Uri(@"http://c7564c85cc00.ngrok.io");
        public static string Token = "";
    }
}
