using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Models;
using carwash.Data;
using carwash.Services;
using RestSharp;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkerRegistrationPage : ContentPage
    {
        public WorkerRegistrationPage()
        {
            InitializeComponent();
        }
        private async void Registration(object sender, EventArgs e)
        {
            if (NamePlaceholder.Text != null && ValidService.nameCheck.IsMatch(NamePlaceholder.Text))
            {
                var workerAnswer = WorkerService.NewWorker(CurrentUserData.Token, NamePlaceholder.Text);
                switch (workerAnswer.Status)
                {
                    case HttpStatusCode.Created:
                        DBService.AddWorker(workerAnswer.Worker);
                        await Navigation.PopModalAsync();
                        break;
                    default:
                        await DisplayAlert("Ошибка регистрации", $"{workerAnswer.Status}", "ОК");
                        break;
                };
            }
            else await DisplayAlert("Ошибка", $"Некорректный ввод имени", "ОK");       
        }
        private void NamePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
        private async void toBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}