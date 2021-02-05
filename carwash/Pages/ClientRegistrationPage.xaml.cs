using carwash.Data;
using carwash.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientRegistrationPage : ContentPage
    {
        public ClientRegistrationPage()
        {
            InitializeComponent();
        }
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void AddNewClient_Clicked(object sender, EventArgs e)
        {
            if (PhonePlaceholder.Text != null && ValidService.numberCheck.IsMatch(PhonePlaceholder.Text))
            {
                if (NamePlaceholder.Text != null && ValidService.nameCheck.IsMatch(NamePlaceholder.Text))
                {
                    var client = ClientService.NewClient(NamePlaceholder.Text, ValidService.ClearPhone(PhonePlaceholder.Text), "{car:auto}", CurrentUserData.Token);
                    switch (client.Status)
                    {
                        case System.Net.HttpStatusCode.OK:
                            DBService.AddOrRewriteClient(client.Client);
                            await Navigation.PopModalAsync();
                            break;
                        case System.Net.HttpStatusCode.Created:
                            DBService.AddOrRewriteClient(client.Client);
                            await Navigation.PopModalAsync();
                            break;
                        default:
                            await DisplayAlert("Ошибка авторизации", $"{client.Status}", "ОK");
                            break;
                    }
                }
                else await DisplayAlert("Ошибка", $"Некорректный ввод имени", "ОK");
            }
            else await DisplayAlert("Ошибка", $"Некорректный ввод номера", "ОK");
        }
        private void NamePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}