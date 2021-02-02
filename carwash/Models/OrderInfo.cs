using System;
using System.Drawing;
using System.ComponentModel;

namespace carwash.Models
{
    public class OrderInfo : INotifyPropertyChanged
    {
        private int clientId;
        private string clientName;
        private string clientPhone;
        private string clientCarInformation;
        private int orderId;
        private DateTime orderDateOfReservation;
        private int orderPrice;
        private string orderType;
        private string orderStatus;
        private int workerId;
        private string workerName;
        private bool isEmpty;
        private Color color;
        public int ClientId 
        {
            get
            {
                return clientId;
            } 
            set 
            {
                if (clientId != value)
                {
                    clientId = value;
                    OnPropertyChanged("ClientId");
                }
            } 
        }
        public string ClientName
        {
            get
            {
                return clientName;
            }
            set
            {
                if (clientName != value)
                {
                    clientName = value;
                    OnPropertyChanged("ClientName");
                }
            }
        }
        public string ClientPhone
        {
            get
            {
                return clientPhone;
            }
            set
            {
                if (clientPhone != value)
                {
                    clientPhone = value;
                    OnPropertyChanged("ClientPhone");
                }
            }
        }
        public string ClientCarInformation
        {
            get
            {
                return clientCarInformation;
            }
            set
            {
                if (clientCarInformation != value)
                {
                    clientCarInformation = value;
                    OnPropertyChanged("ClientCarInformation");
                }
            }
        }
        public int OrderId
        {
            get
            {
                return orderId;
            }
            set
            {
                if (orderId != value)
                {
                    orderId = value;
                    OnPropertyChanged("OrderId");
                }
            }
        }
        public DateTime OrderDateOfReservation { get; }
        public int OrderPrice
        {
            get
            {
                return orderPrice;
            }
            set
            {
                if (orderPrice != value)
                {
                    orderPrice = value;
                    OnPropertyChanged("OrderPrice");
                }
            }
        }
        public string OrderType
        {
            get
            {
                return orderType;
            }
            set
            {
                if (orderType != value)
                {
                    orderType = value;
                    OnPropertyChanged("OrderType");
                }
            }
        }
        public string OrderStatus
        {
            get
            {
                return orderStatus;
            }
            set
            {
                if (orderStatus != value)
                {
                    orderStatus = value;
                    OnPropertyChanged("OrderStatus");
                }
            }
        }
        public int WorkerId
        {
            get
            {
                return workerId;
            }
            set
            {
                if (workerId != value)
                {
                    workerId = value;
                    OnPropertyChanged("WorkerId");
                }
            }
        }
        public string WorkerName
        {
            get
            {
                return workerName;
            }
            set
            {
                if (workerName != value)
                {
                    workerName = value;
                    OnPropertyChanged("WorkerName");
                }
            }
        }
        public bool IsEmpty
        {
            get
            {
                return isEmpty;
            }
            set
            {
                if (isEmpty != value)
                {
                    isEmpty = value;
                    OnPropertyChanged("IsEmpty");
                }
            }
        }
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged("Color");
                }
            }
        }
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
            IsEmpty = false;
            Color = fullInfo;
        }
        public OrderInfo(DateTime DateOfReservation)
        {
            OrderDateOfReservation = DateOfReservation;
            OrderId = -1;
            OrderPrice = -1;
            OrderType = "";
            OrderStatus = "";
            ClientCarInformation = "";
            ClientId = -1;
            ClientName = "Свободно";
            ClientPhone = "";
            WorkerId = -1;
            WorkerName = "";
            IsEmpty = true;
            Color = emptyInfo;
        }
        public void FillingInfo(OrderInfo newInfo)
        {
            OrderId = newInfo.OrderId;           
            OrderPrice = newInfo.OrderPrice;
            OrderType = newInfo.OrderType;
            OrderStatus = newInfo.OrderStatus;
            ClientCarInformation = newInfo.ClientCarInformation;
            ClientId = newInfo.ClientId;
            ClientName = newInfo.ClientName;
            ClientPhone = newInfo.ClientPhone;
            WorkerId = newInfo.WorkerId;
            WorkerName = newInfo.WorkerName;
            IsEmpty = false;
            Color = fullInfo;
        }
        public void ClearInfo()
        {
            OrderId = -1;
            OrderPrice = -1;
            OrderType = "";
            OrderStatus = "";
            ClientCarInformation = "";
            ClientId = -1;
            ClientName = "Свободно";
            ClientPhone = "";
            WorkerId = -1;
            WorkerName = "";
            IsEmpty = true;
            Color = emptyInfo;
        }
        private Color emptyInfo = Color.FromArgb(80, 206, 212, 255);
        private Color fullInfo = Color.FromArgb(80, 255, 206, 206);
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
