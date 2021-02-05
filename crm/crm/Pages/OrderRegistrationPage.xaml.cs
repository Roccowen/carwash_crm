using carwash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Services;
using carwash.Data;


namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderRegistrationPage : ContentPage
    {
        public List<Client> Clients { get; set; }
        //public List<Worker> Workers { get; set; }
        private DateTime pickedDate { get; set; }
        private OrderInfo pickedOrderInfo { get; set; }
        //public Worker CurrentWorker { get; set; }
        private Client currentClient;
        public Client CurrentClient 
        { 
            get 
            {
                return currentClient;
            } 
            set 
            {
                if (value != currentClient)
                {
                    currentClient = value;
                    OnPropertyChanged("CurrentClient");
                }              
            } 
        }
        public OrderRegistrationPage(OrderInfo _pickedOrderInfo, DateTime? _pickedDateTime=null)
        {
            InitializeComponent();
            pickedOrderInfo = _pickedOrderInfo;
            pickedDate = _pickedDateTime ?? _pickedDateTime.Value;

            Clients = DBService.GetClients();
            //Workers = DBService.GetWorkers();

            if (pickedOrderInfo.OrderPrice>0)
                PricePlaceholder.Text = pickedOrderInfo.OrderPrice.ToString();
            ReservationDataPicker.Date = pickedDate;
            ReservationTimePicker.Time = pickedOrderInfo.OrderDateOfReservation.TimeOfDay;
            
            //var pickedWorker = Workers.FirstOrDefault(w => w.Id == _pickedOrderInfo.WorkerId);
            //if (pickedWorker != null)
            //    CurrentWorker = pickedWorker;
            var pickedClient = Clients.FirstOrDefault(w => w.Id == _pickedOrderInfo.ClientId);
            if (pickedClient != null)
                CurrentClient = pickedClient;

            this.BindingContext = this;
        }
        private void AddNewOrder_Clicked(object sender, EventArgs e)
        {
            int price = -1;
            //if (WorkerPicker.SelectedItem != null)
            {
                //if (ClientPicker.SelectedItem != null)
                if (CurrentClient != null)
                {
                    if (int.TryParse(PricePlaceholder.Text, out price) && price > 0)
                    {
                        if (CurrentUserData.Token != "")
                        {
                            var reservationDateTime = ReservationDataPicker.Date.Add(ReservationTimePicker.Time);
                            var client = CurrentClient;
                            var worker = DBService.GetWorker(1);
                            //var worker = (Worker)WorkerPicker.SelectedItem;
                            (System.Net.HttpStatusCode Status, Order Order) order = (System.Net.HttpStatusCode.Unused, null);
                            if (!pickedOrderInfo.IsEmpty)
                                order = OrderService.ChangeOrder(pickedOrderInfo.OrderId, CurrentUserData.Token, pickedOrderInfo.OrderDateOfReservation, client.Id, worker.Id, Convert.ToInt32(PricePlaceholder.Text), pickedOrderInfo.OrderType, pickedOrderInfo.OrderStatus);
                            else
                                order = OrderService.NewOrder(reservationDateTime, client.Id, worker.Id, price, CurrentUserData.Token);
                            switch (order.Status)
                            {
                                case System.Net.HttpStatusCode.Created:
                                    DBService.AddOrRewriteOrder(order.Order);
                                    pickedOrderInfo.FillingInfo(new OrderInfo(order.Order, client, worker));
                                    Navigation.PopModalAsync();
                                    break;
                                case System.Net.HttpStatusCode.OK:
                                    DBService.AddOrRewriteOrder(order.Order);
                                    pickedOrderInfo.FillingInfo(new OrderInfo(order.Order, client, worker));
                                    Navigation.PopModalAsync();
                                    break;
                                default:
                                    DisplayAlert("Ошибка соединения", $"{order.Status}", "ОК");
                                    break;
                            }
                        }
                    }
                    else DisplayAlert("Ошибка", "Цена введена некорректно", "ОК");
                }
                else DisplayAlert("Ошибка", "Необходимо выбрать клиента", "ОК");
            }
            //else DisplayAlert("Ошибка", "Необходимо выбрать рабочего", "ОК");
        }
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private void PricePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ClientPicker_Focused(object sender, FocusEventArgs e)
        {
            
        }

        private async void ClientPickerButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SearchOrCreatePage(this));
        }
    }
}