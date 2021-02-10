using carwash.Data;
using carwash.Models;
using carwash.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderRegistrationPage : ContentPage
    {
        public List<Client> Clients { get; set; }
        public List<Worker> Workers { get; set; }
        private OrdersPage ordersPageParent { get; set; }
        private DateTime pickedDateTime { get; set; }
        public OrderRegistrationPage(OrdersPage ordersPage, DateTime? _pickedDateTime = null)
        {
            InitializeComponent();
            ordersPageParent = ordersPage;
            pickedDateTime = _pickedDateTime ?? _pickedDateTime.Value;
            ReservationDataPicker.Date = pickedDateTime;
            Clients = DBService.GetClients();
            Workers = DBService.GetWorkers();
            this.BindingContext = this;
        }
        private async void AddNewOrder_Clicked(object sender, EventArgs e)
        {
            int price = -1;
            if (WorkerPicker.SelectedItem != null)
            {
                if (ClientPicker.SelectedItem != null)
                {
                    if (int.TryParse(PricePlaceholder.Text, out price) && price > 0)
                    {
                        if (UserData.Token != "")
                        {
                            var reservationDateTime = ReservationDataPicker.Date.Add(ReservationTimePicker.Time);
                            var client = (Client)ClientPicker.SelectedItem;
                            var worker = (Worker)WorkerPicker.SelectedItem;
                            var order = OrderService.NewOrder(reservationDateTime, client.Id, worker.Id, price, UserData.Token);
                            switch (order.Status)
                            {
                                case System.Net.HttpStatusCode.Created:
                                    DBService.AddOrRewriteOrder(order.Order);
                                    ordersPageParent.ordersInfo.Add(new OrderInfo(order.Order, client, worker));
                                    await Navigation.PopModalAsync();
                                    break;
                                case System.Net.HttpStatusCode.OK:
                                    DBService.AddOrRewriteOrder(order.Order);
                                    ordersPageParent.ordersInfo.Add(new OrderInfo(order.Order, client, worker));
                                    await Navigation.PopModalAsync();
                                    break;
                                default:
                                    await DisplayAlert("Ошибка добавления заказа", $"{order.Status}", "ОК");
                                    break;
                            }
                        }
                    }
                    else await DisplayAlert("Ошибка", "Цена введена некорректно", "ОК");
                }
                else await DisplayAlert("Ошибка", "Необходимо выбрать клиента", "ОК");
            }
            else await DisplayAlert("Ошибка", "Необходимо выбрать рабочего", "ОК");
        }
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private void PricePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}