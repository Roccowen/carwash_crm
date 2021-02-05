using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Models;
using carwash.Services;
using carwash.Data;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientRegistrationPage : ContentPage
    {
        private OrderRegistrationPage _parentPage { get; set; } = null;
        private SearchOrCreatePage _searchOrCreatePage { get; set; } = null;
        public ClientRegistrationPage(string newclientname, OrderRegistrationPage parentPage = null, SearchOrCreatePage searchOrCreatePage= null)
        {
            InitializeComponent();

            NamePlaceholder.Text = newclientname;


            if (parentPage != null)
                _parentPage = parentPage;
            if (searchOrCreatePage != null)
                _searchOrCreatePage = searchOrCreatePage;
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
                            if (_parentPage != null)
                                _parentPage.CurrentClient = client.Client;
                            DBService.AddOrRewriteClient(client.Client);
                            await Navigation.PopModalAsync();
                            if (_searchOrCreatePage != null)
                                _searchOrCreatePage.ClosePage();
                                break;
                        case System.Net.HttpStatusCode.Created:
                            if (_parentPage != null)
                                _parentPage.CurrentClient = client.Client;
                            DBService.AddOrRewriteClient(client.Client);
                            await Navigation.PopModalAsync();
                            if (_searchOrCreatePage != null)
                                _searchOrCreatePage.ClosePage();
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