using System;
using System.Collections.Generic;
using System.Text;
using carwash.Data;
using carwash.Services;
namespace carwash
{
    public static class Test
    {
        public static bool withRegistration = false;
        public static bool useApi = true;
        public static bool useLocal = !useApi;
        public static void InitialParams()
        {
            //CurrentUserData.ClearData();
            //DBService.DropData();
            //System.Diagnostics.Debug.WriteLine($"${CurrentUserData.Name}$-----------------------------");
            //System.Diagnostics.Debug.WriteLine($"${CurrentUserData.Name}$-----------------------------");
        }
    }
}
