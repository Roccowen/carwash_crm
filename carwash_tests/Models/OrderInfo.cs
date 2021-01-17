using System;

namespace carwash.Models
{
    public class OrderInfo
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string ClientCarInformation { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDateOfReservation { get; set; }
        public int OrderPrice { get; set; }
        public string OrderType { get; set; }
        public string OrderStatus { get; set; }
        public int WorkerId { get; set; }
        public string WorkerName { get; set; }
        public OrderInfo(Order order, Client client, Worker worker)
        {
            OrderDateOfReservation = order.DateOfReservation;
            OrderId = order.Id;
            OrderPrice = order.Price;
            OrderType = order.Type;
            OrderStatus = order.Status;
            ClientCarInformation = client.CarInformation;
            ClientId = client.Id;
            ClientName = client.Name;
            ClientPhone = client.Phone;
            WorkerId = worker.Id;
            WorkerName = worker.Name;
        }

    }
}
