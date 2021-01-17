using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using carwash.Data;
using carwash.Models;
using carwash.Services;
using carwash.Pages;
using System.Net;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedMainPage : TabbedPage
    {
        public TabbedMainPage()
        {
            InitializeComponent();
            if (CurrentUserData.Token != "")
            {
                System.Diagnostics.Debug.WriteLine("@MP Token is not empty");
                var currentUserAnswer = UserService.GetCurrentUser(CurrentUserData.Token);
                if (currentUserAnswer.Status == HttpStatusCode.OK)
                {
                    CurrentUserData.NewUserData(currentUserAnswer.User);
                    System.Diagnostics.Debug.WriteLine("@MP threading start");
                    var orders = new List<Order>();
                    var clients = new List<Client>();
                    var workers = new List<Worker>();
                    System.Diagnostics.Debug.WriteLine("@MP ordersTask is start");
                    Task.Factory.StartNew(() =>
                    {
                        orders = OrderService.GetOrdersDebug(CurrentUserData.Token).Orders;
                        workers = WorkerService.GetWorkers(CurrentUserData.Token).Workers;
                        clients = ClientService.GetClients(CurrentUserData.Token).Clients;
                    }).ContinueWith(task =>
                    {
                        DBService.DBFilling(orders, workers, clients);
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                    CurrentUserData.Id = -1;
            }
            if (CurrentUserData.Id == -1 || CurrentUserData.Token == "")
            {
                System.Diagnostics.Debug.WriteLine("@Token is empty or Id is -1");
                Navigation.PushModalAsync(new AuthorizationPage());
            }
        }
    }
}