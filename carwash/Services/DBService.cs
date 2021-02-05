using carwash.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                foreach (var client in clients)
                    AddOrRewriteClient(client);
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
                foreach (var worker in workers)
                    AddOrRewriteWorker(worker);
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
            try
            {
                foreach (var order in orders)
                    AddOrRewriteOrder(order);
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
        public static Worker GetWorker(int id)
        {
            var context = new DBContext();
            return context.Workers.Find(id);
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
        public static void AddOrRewriteClient(Client client)
        {
            DBContext context = new DBContext();
            var c = context.Clients.Find(client.Id);
            if (c is null)
            {
                context.Clients.Add(client);
                System.Diagnostics.Debug.WriteLine($"@Added new client {client.Id}-{client.Name}");
                context.SaveChanges();
            }
            else
            {
                c.Phone = client.Phone;
                c.CarInformation = client.CarInformation;
                c.Name = client.Name;
                
                System.Diagnostics.Debug.WriteLine($"@Not added new client {client.Id}-{client.Name}");
                context.SaveChanges();
            }
        }
        public static void AddOrRewriteWorker(Worker worker)
        {
            DBContext context = new DBContext();
            var w = context.Workers.Find(worker.Id);
            if (w is null)
            {
                context.Workers.Add(worker);
                System.Diagnostics.Debug.WriteLine($"@Added new worker {worker.Id}-{worker.Name}");
                context.SaveChanges();
            }
            else
            {
                w.Name = worker.Name;
                System.Diagnostics.Debug.WriteLine($"@Worker was rewrited {worker.Id}-{worker.Name}");
                context.SaveChanges();
            }

        }
        public static void AddOrRewriteOrder(Order order)
        {
            DBContext context = new DBContext();
            var client = context.Clients.Find(order.ClientId);
            if (client != null)
            {
                System.Diagnostics.Debug.WriteLine($"@FindOrAddOrder(Order order) client is not null {client.Id}-{client.Name}");
                var worker = context.Workers.Find(order.WorkerId);
                if (worker != null)
                {
                    var o = context.Orders.Find(order.Id);
                    if (o is null)
                    {
                        System.Diagnostics.Debug.WriteLine("@FindOrAddOrder(Order order) order is null");
                        System.Diagnostics.Debug.WriteLine($"@FindOrAddOrder(Order order) worker is not null {worker.Id}-{worker.Name}");
                        order.Client = client;
                        order.Worker = worker;
                        context.Orders.Add(order);
                        context.SaveChanges();
                        System.Diagnostics.Debug.WriteLine($"@FindOrAddOrder(Order order) add new orders-{order.Id}");
                    }
                    else
                    {                     
                        o.ClientId = order.ClientId;
                        o.DateOfReservation = order.DateOfReservation;
                        o.Price = order.Price;
                        o.Status = order.Status;
                        o.WorkerId = order.WorkerId;
                        o.Worker = worker;
                        o.Client = client;
                        //context.Update(o);
                        context.SaveChanges();
                        System.Diagnostics.Debug.WriteLine($"@FindOrAddOrder(Order order) order was rewrite order-{order.Id}");
                    }

                }
                else
                    System.Diagnostics.Debug.WriteLine($"@FindOrAddOrder(Order order) can't find worker {order.WorkerId} skip order-{order.Id}");
            }
            else
                System.Diagnostics.Debug.WriteLine($"@FindOrAddOrder(Order order) can't find client {order.ClientId}  skip order-{order.Id}");
        }
        public static Order GetOrderById(int id)
        {
            DBContext context = new DBContext();
            var order = context.Orders.Find(id);
            return order;
        }
        public static List<Worker> GetWorkers()
        {
            DBContext context = new DBContext();
            return context.Workers.ToList();
        }
        public static void DelOrderById(int id)
        {
            DBContext context = new DBContext();
            var o = context.Orders.Find(id);
            context.Orders.Remove(o);
            context.SaveChanges();
        }
        public static List<Client> GetClients()
        {
            DBContext context = new DBContext();
            return context.Clients.ToList();
        }
        public static List<OrderInfo> GetSortedOrderForDateInfos(DateTime? date = null)
        {           
            var today = date is null ? 
                DateTime.Now.Date : date.Value;      
            DBContext context = new DBContext();
            System.Diagnostics.Debug.WriteLine($"@GetOrderInfos workers-{context.Workers.Count()}");
            System.Diagnostics.Debug.WriteLine($"@GetOrderInfos clients-{context.Clients.Count()}");
            System.Diagnostics.Debug.WriteLine($"@GetOrderInfos orders-{context.Orders.Count()}");
            var inofs = new List<OrderInfo>();
            List<Order> orders = context.Orders
                .Where(i => i.DateOfReservation.Year == today.Year &&
                            i.DateOfReservation.Month == today.Month &&
                            i.DateOfReservation.Day == today.Day)
                .Include(o => o.Worker)
                .Include(o => o.Client)
                .OrderBy(o => o.DateOfReservation)
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
