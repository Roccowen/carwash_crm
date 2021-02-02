using carwash.Data;
using carwash.Models;
using carwash.Pages;
using carwash.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace carwash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorizationPage : ContentPage
    {
        public AuthorizationPage()
        {
            InitializeComponent();
            if (CurrentUserData.Token != "")
            {
                System.Diagnostics.Debug.WriteLine("@AUTH_INIT Token is not empty");
                var currentUserAnswer = UserService.GetCurrentUser(CurrentUserData.Token);
                if (currentUserAnswer.Status == HttpStatusCode.OK)
                {
                    CurrentUserData.NewUserData(currentUserAnswer.User);
                    System.Diagnostics.Debug.WriteLine("@AUTH_INIT threading start");
                    var orders = new List<Order>();
                    var clients = new List<Client>();
                    var workers = new List<Worker>();
                    System.Diagnostics.Debug.WriteLine("@AUTH_INIT ordersTask is start");
                    //var orderTask = Task.Factory.StartNew(() =>
                    //{
                    //    orders = OrderService.GetOrders(CurrentUserData.Token).Orders;
                    //});
                    var workerTask = Task.Factory.StartNew(() =>
                    {
                        workers = WorkerService.GetWorkers(CurrentUserData.Token).Workers;
                    });
                    var clientsTask = Task.Factory.StartNew(() =>
                    {
                        clients = ClientService.GetClients(CurrentUserData.Token).Clients;
                    });
                    //Task.WaitAll(orderTask, workerTask, clientsTask);
                    Task.WaitAll(workerTask, clientsTask);
                    orders = OrderService.GetOrders(CurrentUserData.Token).Orders;
                    if (clients == null)
                    {
                        throw new Exception();
                    }
                    DBService.DBFilling(orders, workers, clients);
                    ClearFields();
                    Navigation.PushModalAsync(new TabbedMainPage());
                }
                else
                    CurrentUserData.Id = -1;
            }
            if (CurrentUserData.Id == -1 || CurrentUserData.Token == "")
            {
                System.Diagnostics.Debug.WriteLine("@Token is empty or Id is -1");
            }
        }
        public void AuthorizationClicked(object sender, EventArgs e)
        {
            if (NumberPlaceholder.Text != null && ValidService.numberCheck.IsMatch(NumberPlaceholder.Text))
            {
                if (PasswordPlaceholder.Text != null && ValidService.passwordCheck.IsMatch(PasswordPlaceholder.Text))
                {
                    var answer = UserService.Authorization(ValidService.ClearPhone(NumberPlaceholder.Text), PasswordPlaceholder.Text);
                    switch (answer.Status)
                    {
                        case System.Net.HttpStatusCode.OK:
                            CurrentUserData.Token = answer.Token;
                            var currentUserAnswer = UserService.GetCurrentUser(CurrentUserData.Token);
                            if (currentUserAnswer.Status == HttpStatusCode.OK)
                            {
                                System.Diagnostics.Debug.WriteLine("@AUTH auth is suc");
                                CurrentUserData.NewUserData(currentUserAnswer.User);

                                System.Diagnostics.Debug.WriteLine("@AUTH threading start");
                                var orders = new List<Order>();
                                var clients = new List<Client>();
                                var workers = new List<Worker>();
                                System.Diagnostics.Debug.WriteLine("@AUTH ordersTask is start");
                                //var orderTask = Task.Factory.StartNew(() =>
                                //{
                                //    orders = OrderService.GetOrders(CurrentUserData.Token).Orders;
                                //});
                                var workerTask = Task.Factory.StartNew(() =>
                                {
                                    workers = WorkerService.GetWorkers(CurrentUserData.Token).Workers;
                                });
                                var clientsTask = Task.Factory.StartNew(() =>
                                {
                                    clients = ClientService.GetClients(CurrentUserData.Token).Clients;
                                });
                                //Task.WaitAll(orderTask, workerTask, clientsTask);
                                Task.WaitAll(workerTask, clientsTask);
                                orders = OrderService.GetOrders(CurrentUserData.Token).Orders;
                                if (clients == null)
                                {
                                    throw new Exception();
                                }
                                DBService.DBFilling(orders, workers, clients);
                                ClearFields();
                                Navigation.PushModalAsync(new TabbedMainPage());
                            }
                            else
                                DisplayAlert("Ошибка получения данных", $"{currentUserAnswer.Status}", "ОK");
                            break;
                        case System.Net.HttpStatusCode.NotFound:
                            DisplayAlert("Ошибка авторизации", $"Неверный номер или пароль", "ОK");
                            break;
                        default:
                            DisplayAlert("Ошибка авторизации", $"{answer.Status}", "ОK");
                            break;
                    }
                }
                else DisplayAlert("Ошибка", $"Пароль должен содержать шесть символов", "ОK");
            }
            else DisplayAlert("Ошибка", $"Некорректный ввод номера", "ОK");
        }
        public async void ToRegistration(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegistrationPage());
        }
        private void NumberPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void PasswordPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void ClearFields()
        {
            NumberPlaceholder.Text = "";
            PasswordPlaceholder.Text = "";
        }
    }
}