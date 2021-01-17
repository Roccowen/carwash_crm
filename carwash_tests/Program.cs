using System;
using carwash.Services;
using carwash.Models;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace carwash_tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentUserToken = UserService.Authorization("7777777777", "qwerty").Token;
            var currentUser = UserService.GetCurrentUser(currentUserToken).User;
            var clients = ClientService.GetClients(currentUserToken).Clients;
            ShowCLients(clients);
            var orders = OrderService.GetOrdersDebug(currentUserToken).Orders;
            ShowOrders(orders);
            var workers = WorkerService.GetWorkers(currentUserToken).Workers;
            ShowWorkers(workers);
            DBService.DBFilling(orders, workers, clients);
            var clientsDB = DBService.GetClients();
            ShowCLients(clientsDB);
            var ordersDB = DBService.GetOrders();
            ShowOrders(ordersDB);
            var workersDB = DBService.GetWorkers();
            ShowWorkers(workersDB);
            Console.ReadKey();

        }
        static void ShowCLients(List<Client> clients)
        {
            foreach (var client in clients)
                Console.WriteLine($"Client - {client.Id} {client.Name} {client.Phone} {client.UserId}");
        }
        static void ShowOrders(List<Order> orders)
        {
            foreach (var order in orders)
                Console.WriteLine($"Order - {order.Id} {order.DateOfReservation} {order.Price} {order.Status} {order.Type} {order.UserId} {order.WorkerId}");
        }
        static void ShowWorkers(List<Worker> workers)
        {
            foreach (var worker in workers)
                Console.WriteLine($"Worker - {worker.Id} {worker.Name} {worker.UserId}");
        }
    }
}
