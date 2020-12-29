using System;
using System.Collections.Generic;
using System.Text;

namespace carwash
{
    public static class Test
    {
        public static bool withRegistration = true;
        public static bool useApi = true;
        public static bool useLocal = !useApi;
        public static void InitialParams()
        {
            App.Current.Properties.Clear();
        }
    }
}
