using System;
using System.Diagnostics;
using carwash.Services;
using carwash.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Linq;

namespace carwash_tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentUserToken = UserService.Authorization("7777777777", "qwerty").Token;
            var currentUser = UserService.GetCurrentUser(currentUserToken).User;
            var clients = ClientService.GetClients(currentUserToken).Clients;
            var_dump(clients);         
            var workers = WorkerService.GetWorkers(currentUserToken).Workers;
            var_dump(workers);
            var orders = OrderService.GetOrders(currentUserToken).Orders;
            var_dump(orders);
        }

        public static void var_dump(Object obj)
        {
            var type = obj.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).OrderBy(f => f.Name);
            Regex regex = new Regex(@"\<.*?\>");
            Debug.Write($"{type} ");
            Console.Write($"{type} ");
            foreach (var field in fields)
            {
                Debug.Write($"{regex.Match(field.Name).Value.Replace("<", "").Replace(">", "")}-{field.GetValue(obj)} ");
                Console.Write($"{regex.Match(field.Name).Value.Replace("<", "").Replace(">", "")}-{field.GetValue(obj)} ");
            }              
            Debug.Write("\n");
            Console.Write("\n");
        }
        public static void var_dump(IEnumerable<object> objs)
        {
            foreach (var obj in objs)
                var_dump(obj);
        }
    }
}
