using carwash.Data;
using carwash.Models;
using carwash.Pages;
using carwash.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace carwash
{
    [DesignTimeVisible(false)]
    public partial class MainPageOld : MasterDetailPage
    {
        public MainPageOld()
        {
            InitializeComponent();

            if (UserData.Token != "")
            {
                System.Diagnostics.Debug.WriteLine("@MP Token is not empty");
                var currentUserAnswer = UserService.GetCurrentUser(UserData.Token);
                if (currentUserAnswer.Status == HttpStatusCode.OK)
                {
                    UserData.NewUserData(currentUserAnswer.User);
                    System.Diagnostics.Debug.WriteLine("@MP threading start");
                    var orders = new List<Order>();
                    var clients = new List<Client>();
                    var workers = new List<Worker>();
                    System.Diagnostics.Debug.WriteLine("@MP ordersTask is start");
                    Task.Factory.StartNew(() =>
                    {
                        orders = OrderService.GetOrders(UserData.Token).Orders;
                        workers = WorkerService.GetWorkers(UserData.Token).Workers;
                        clients = ClientService.GetClients(UserData.Token).Clients;
                    }).ContinueWith(task =>
                    {
                        DBService.DBFilling(orders, workers, clients);
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                    UserData.Id = -1;
            }
            if (UserData.Id == -1 || UserData.Token == "")
            {
                System.Diagnostics.Debug.WriteLine("@Token is not empty or Id is -1");
                Navigation.PushModalAsync(new AuthorizationPage());
            }

            Detail = new NavigationPage(new MainPage());
            IsPresented = false;
        }
        public void SettingsButtonClick(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new SettingsPage());
            IsPresented = false;
        }
        public void AboutButtonClick(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new MainPage());
            IsPresented = false;
        }
        public void OrderButtonClick(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new OrdersPage());
            IsPresented = false;
        }
        private void WorkersButton_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new WorkersPage());
            IsPresented = false;
        }
    }
}
