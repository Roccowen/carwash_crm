using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using carwash.Data;
using carwash.Models;
using carwash.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersPage : ContentPage
    {
        public ObservableCollection<OrderInfo> ordersInfo { get; set; }

        public OrdersPage()
        {
            InitializeComponent();

            ordersInfo = new ObservableCollection<OrderInfo>();
            var ordersAll = OrderService.GetOrdersDebug(CurrentUserData.Token);
            System.Diagnostics.Debug.WriteLine($"@Getted orders count - {ordersAll.Orders.Count}");
            string today = DateTime.Now.Date.ToString("yyyy-MM-dd");
            System.Diagnostics.Debug.WriteLine($"@Today - {today}");
            foreach (var order in ordersAll.Orders)
                System.Diagnostics.Debug.WriteLine($"@Order{order.Id} - {order.DateOfReservation.Date.ToString("yyyy-MM-dd")}");
            var orders = ordersAll.Orders.Where(i => i.DateOfReservation.Date.ToString("yyyy-MM-dd") == today).ToList();
            System.Diagnostics.Debug.WriteLine($"@Today orders count - {orders.Count}");
            if (orders.Count != 0)
            {
                OrdersIsNullLabel.IsVisible = false;
                foreach (var currentOrder in orders)
                {
                    var currentClient = ClientService.GetClientsById(currentOrder.ClientId, CurrentUserData.Token);
                    var currentWorker = WorkerService.GetWorkerById(currentOrder.WorkerId, CurrentUserData.Token);
                    if (currentClient.Client != null && currentWorker.Worker != null)
                        ordersInfo.Add(new OrderInfo(
                        currentOrder,
                        currentClient.Client,
                        currentWorker.Worker
                    ));
                }
            }
            else
                OrdersIsNullLabel.IsVisible = true;
            this.BindingContext = this;       
        }
        private async void OrdersDataPicker_Unfocused(object sender, FocusEventArgs e)
        {
            ordersInfo.Clear();
            System.Diagnostics.Debug.WriteLine($"@{ordersInfo.Count}");
            var ordersAll = OrderService.GetOrdersDebug(CurrentUserData.Token);
            var orders = ordersAll.Orders.Where(i => i.DateOfReservation.Date.ToString("yyyy-MM-dd")== OrdersDataPicker.Date.ToString("yyyy-MM-dd")).ToList();
            if (orders.Count != 0)
            {
                System.Diagnostics.Debug.WriteLine("@orders.Count > 0");
                OrdersIsNullLabel.IsVisible = false;
                foreach (var currentOrder in orders)
                {
                    var currentClientTask = ClientService.GetClientsByIdAsync(currentOrder.ClientId, CurrentUserData.Token);
                    var currentWorkerTask = WorkerService.GetWorkerByIdAsync(currentOrder.WorkerId, CurrentUserData.Token);
                    var currentClient = await currentClientTask;
                    var currentWorker = await currentWorkerTask;
                    if (currentClient.Client != null && currentWorker.Worker != null)
                        ordersInfo.Add(new OrderInfo(
                        currentOrder,
                        currentClient.Client,
                        currentWorker.Worker
                        ));
                }
                this.BindingContext = this;
            }
            else
                OrdersIsNullLabel.IsVisible = true;
        }
        private async void NewOrderButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new OrderRegistrationPage());
        }

        private void OrdersDataPicker_Focused(object sender, FocusEventArgs e)
        {

        }
    }
}