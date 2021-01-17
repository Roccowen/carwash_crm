using System;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Models;
using carwash.Services;
using carwash.Data;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace carwash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorizationPage : ContentPage
    {
        public AuthorizationPage()
        {
            InitializeComponent();
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
                                Task.Factory.StartNew(() =>
                                {
                                    System.Diagnostics.Debug.WriteLine("@AUTH ordersTask is start");
                                    orders = OrderService.GetOrdersDebug(CurrentUserData.Token).Orders;
                                    workers = WorkerService.GetWorkers(CurrentUserData.Token).Workers;
                                    clients = ClientService.GetClients(CurrentUserData.Token).Clients;
                                }).ContinueWith(task =>
                                {
                                    DBService.DBFilling(orders, workers, clients);
                                }, TaskScheduler.FromCurrentSynchronizationContext());
                                Navigation.PopModalAsync();
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
    }
}