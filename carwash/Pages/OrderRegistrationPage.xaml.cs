using carwash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Services;
using carwash.Data;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderRegistrationPage : ContentPage
    {
        public List<Client> Clients { get; set; }
        public List<Worker> Workers { get; set; }
        public OrderRegistrationPage()
        {
            InitializeComponent();
            Clients = ClientService.GetClients(CurrentUserData.Token).Clients.Where(c => Convert.ToInt32(c.UserId) == CurrentUserData.Id).ToList();
            Workers = WorkerService.GetWorkers(CurrentUserData.Token).Workers;
            this.BindingContext = this;
        }

        private void PricePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private async void  AddNewOrder_Clicked(object sender, EventArgs e)
        {
            int price = 0;
            if (int.TryParse(PricePlaceholder.Text, out price) &&
                ClientPicker.SelectedItem != null &&
                WorkerPicker.SelectedItem != null &&
                CurrentUserData.Token != "")
            {
                var reservationDateTime = ReservationDataPicker.Date.Add(ReservationTimePicker.Time);
                var client = (Client)ClientPicker.SelectedItem;
                var worker = (Worker)WorkerPicker.SelectedItem;
                var order = OrderService.NewOrder(reservationDateTime, client.Id, worker.Id, price, CurrentUserData.Token);
                System.Diagnostics.Debug.WriteLine($"@{order.Status.ToString()}");
                switch (order.Status)
                {
                    case System.Net.HttpStatusCode.Created:
                        await Navigation.PopModalAsync();
                        break;
                    case System.Net.HttpStatusCode.OK:
                        await Navigation.PopModalAsync();
                        break;
                    default:
                        break;
                }
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}