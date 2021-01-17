using carwash.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace carwash.Services
{
    public static class DBService
    {
        public static void DBFilling(List<Order> orders, List<Worker> workers, List<Client> clients)
        {
            var context = new DBContext();         
            try
            {
                context.Clients.AddRange(clients);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
                System.Diagnostics.Debug.Fail(e.Message);
                System.Diagnostics.Debug.Fail(e.InnerException.Message);
                throw;
            }        
            try
            {
                context.Workers.AddRange(workers);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
                System.Diagnostics.Debug.Fail(e.Message);
                System.Diagnostics.Debug.Fail(e.InnerException.Message);
                throw;
            }
            System.Diagnostics.Debug.WriteLine("@DBFilling clients and workers is added");
            
            foreach (var order in orders)
            {
                if (context.Orders.Find(order.Id) == null)
                {
                    System.Diagnostics.Debug.WriteLine("@DBFilling order is null");
                    var client = context.Clients.Find(order.ClientId);
                    if (client != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"@DBFilling client is not null {client.Id}-{client.Name}");
                        var worker = context.Workers.Find(order.WorkerId);
                        if (worker != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"@DBFilling worker is not null {worker.Id}-{worker.Name}");
                            order.Client = client;
                            order.Worker = worker;
                            context.Orders.Add(order);                            
                            System.Diagnostics.Debug.WriteLine($"@DBFilling order is added {order.Id}");
                            try
                            {
                                context.SaveChanges();
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine(e.Message);
                                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
                                System.Diagnostics.Debug.Fail(e.Message);
                                System.Diagnostics.Debug.Fail(e.InnerException.Message);
                                throw;
                            }                        
                        }
                        else
                            System.Diagnostics.Debug.WriteLine($"@DBFilling cant find worker {order.WorkerId} skip order {order.Id}");
                    }
                    else
                        System.Diagnostics.Debug.WriteLine($"@DBFilling cant find client {order.ClientId} skip order {order.Id}");
                }
                else
                    System.Diagnostics.Debug.WriteLine($"@DBFilling order is not null skip order {order.Id}");
            }           
        }
        public static async Task DBFillingAsync(List<Order> orders, List<Worker> workers, List<Client> clients)
        {
            var context = new DBContext();
            var clientsTask = context.Clients.AddRangeAsync(clients);
            var workersTask = context.Workers.AddRangeAsync(workers);
            await clientsTask;
            await workersTask;
            System.Diagnostics.Debug.WriteLine("@DBFilling clients and workers is added");
            context.SaveChanges();
            var ordersTaks = new Task(async () =>
            {
                foreach (var order in orders)
                {
                    var orderTask = context.Orders.FindAsync(order.Id);
                    var clientTask = context.Clients.FindAsync(order.ClientId);
                    var workerTask = context.Workers.FindAsync(order.WorkerId);
                    var _order = await orderTask;
                    var client = await clientTask;
                    var worker = await workerTask;
                    if (_order == null && client != null && worker != null)
                    {
                        order.Client = client;
                        order.Worker = worker;
                        context.Orders.Add(_order);
                        System.Diagnostics.Debug.WriteLine($"@DBFilling order is added {order.Id}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"@DBFilling order is not added OrderId-{order.Id} WorkerId-{worker.Id} ClientId-{client.Id}");
                    }
                }
            });
            context.SaveChanges();
        }
        public static Worker GetWorker(int id)
        {
            var context = new DBContext();
            return context.Workers.Find(id);
        }
        public static async Task<Worker> GetWorkerAsync(int id)
        {
            var context = new DBContext();
            var workerTask = context.Workers.FindAsync(id);
            return await workerTask;
        }
        public static Client GetClient(int id)
        {
            var context = new DBContext();
            return context.Clients.Find(id);
        }
        public static async Task<Client> GetClientAsync(int id)
        {
            var context = new DBContext();
            var clientTask = context.Clients.FindAsync(id);
            return await clientTask;
        }
        public static async Task AddWorkerAsync(Worker worker)
        {
            var context = new DBContext();
            await context.Workers.AddAsync(worker);
            context.SaveChanges();
        }
        public static async Task AddClientAsync(Client client)
        {
            var context = new DBContext();
            await context.Clients.AddAsync(client);
            context.SaveChanges();
        }
        public static void AddWorker(Worker worker)
        {
            var context = new DBContext();
            context.Workers.Add(worker);
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Fail(e.Message);
                System.Diagnostics.Debug.Fail(e.InnerException.Message);
                throw;
            }
        }
        public static void AddClient(Client client)
        {
            var context = new DBContext();
            context.Clients.Add(client);
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Fail(e.Message);
                System.Diagnostics.Debug.Fail(e.InnerException.Message);
                throw;
            }
        }
        public static void FindOrAddWorker(Worker worker)
        {
            DBContext context = new DBContext();
            if (context.Workers.Find(worker.Id) is null)
            {
                context.Workers.Add(worker);
                System.Diagnostics.Debug.WriteLine($"@Added new worker {worker.Id}-{worker.Name}");
                context.SaveChanges();
            }
            else System.Diagnostics.Debug.WriteLine($"@Not added new worker {worker.Id}-{worker.Name}");
        }
        public static void FindOrAddClient(Client client)
        {
            DBContext context = new DBContext();
            if (context.Clients.Find(client.Id) is null)
            {
                context.Clients.Add(client);
                System.Diagnostics.Debug.WriteLine($"@Added new client {client.Id}-{client.Name}");
                context.SaveChanges();
            }
            else System.Diagnostics.Debug.WriteLine($"@Not added new client {client.Id}-{client.Name}");
        }
        public static void FindOrAddOrder(Order order)
        {
            DBContext context = new DBContext();           
            if (context.Orders.Find(order.Id) == null)
            {
                System.Diagnostics.Debug.WriteLine("@FindOrAddOrder(Order order) order is null");
                var client = context.Clients.Find(order.ClientId);
                if (client != null)
                {
                    System.Diagnostics.Debug.WriteLine($"@FindOrAddOrder(Order order) client is not null {client.Id}-{client.Name}");
                    var worker = context.Workers.Find(order.WorkerId);
                    if (worker != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"@FindOrAddOrder(Order order) worker is not null {worker.Id}-{worker.Name}");
                        order.Client = client;
                        order.Worker = worker;
                        context.Orders.Add(order);
                        context.SaveChanges();
                    }
                    else
                        System.Diagnostics.Debug.WriteLine($"@FindOrAddOrder(Order order) can't find worker {order.WorkerId} skip order-{order.Id}");
                }
                else
                    System.Diagnostics.Debug.WriteLine($"@FindOrAddOrder(Order order) can't find client {order.ClientId}  skip order-{order.Id}");
            }
            else
                System.Diagnostics.Debug.WriteLine($"@FindOrAddOrder(Order order) order is not null skip order-{order.Id}");
        }
        public static async Task FindOrAddOrderAsync(Order _order)
        {
            DBContext context = new DBContext();
            var orderTask = context.Orders.FindAsync(_order.Id);
            var clientTask = context.Clients.FindAsync(_order.ClientId);
            var workerTask = context.Workers.FindAsync(_order.WorkerId);
            var order = await orderTask;
            var client = await clientTask;
            var worker = await workerTask;
            if (order == null && client != null && worker != null)
            {
                _order.Client = client;
                _order.Worker = worker;
                context.Orders.Add(_order);
                context.SaveChanges();
            }
        }
        public static void FindOrAddOrder(Order _order, Worker _worker, Client _client)
        {
            DBContext context = new DBContext();
            if (_order != null && _client != null && _worker != null)
            {
                _order.Client = _client;
                _order.Worker = _worker;
                context.Orders.Add(_order);
                context.SaveChanges();
            }
        }
        public static void AddWorkers(List<Worker> workers)
        {
            DBContext context = new DBContext();
            context.Workers.AddRange(workers);
            context.SaveChanges();
        }
        public static void AddClients(List<Client> clients)
        {
            DBContext context = new DBContext();
            context.Clients.AddRange(clients);
            context.SaveChanges();
        }
        public static List<Worker> GetWorkers()
        {
            DBContext context = new DBContext();
            return context.Workers.ToList();
        }
        public static List<Client> GetClients()
        {
            DBContext context = new DBContext();
            return context.Clients.ToList();
        }
        public static List<Order> GetOrders()
        {
            DBContext context = new DBContext();
            return context.Orders.ToList();
        }
        public static List<OrderInfo> GetOrderInfos()
        {
            DBContext context = new DBContext();
            System.Diagnostics.Debug.WriteLine($"@GetOrderInfos workers-{context.Workers.Count()}");
            System.Diagnostics.Debug.WriteLine($"@GetOrderInfos clients-{context.Clients.Count()}");
            System.Diagnostics.Debug.WriteLine($"@GetOrderInfos orders-{context.Orders.Count()}");
            var inofs = new List<OrderInfo>();
            List<Order> orders = context.Orders
                .Include(o => o.Worker)
                .Include(o => o.Client)
                .ToList();
            foreach (var _order in orders)
            {
                if (_order != null && _order.Client != null && _order.Worker != null)
                    inofs.Add(new OrderInfo(_order, _order.Client, _order.Worker));
                else
                    System.Diagnostics.Debug.WriteLine($"@GetOrderInfos order-{_order.Id} had nulls");
            }               
            return inofs;
        }
        public static void DropData()
        {
            DBContext context = new DBContext();
            context.Database.EnsureDeleted();
            context.SaveChanges();
        }
    }
}
