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
using crm.Pages;
using System.Net;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientRegistrationPage : ContentPage
    {
        private OrderRegistrationPage parentPage { get; set; } = null;
        private ClientsSearchPage searchOrCreatePage { get; set; } = null;
        private СlientsPage сlientsPage { get; set; } = null;
        private Client Client { get; set; } = null;
        private bool isUpdate { get; set; } = false;
        public ClientRegistrationPage()
        {
            InitializeComponent();
        }
        public ClientRegistrationPage(string newclientname, OrderRegistrationPage _parentPage, ClientsSearchPage _searchOrCreatePage)
        {
            InitializeComponent();
            NamePlaceholder.Text = newclientname;
            parentPage = _parentPage;
            searchOrCreatePage = _searchOrCreatePage;
        }
        public ClientRegistrationPage(Client client, СlientsPage _clientPage)
        {
            InitializeComponent();
            сlientsPage = _clientPage;
            Client = client;
            NamePlaceholder.Text = client.Name;
            PhonePlaceholder.Text = client.Phone;
        }
        public ClientRegistrationPage(СlientsPage _clientPage)
        {
            InitializeComponent();
            сlientsPage = _clientPage;
        }
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void AddNewClient_Clicked(object sender, EventArgs e)
        {            
            if (PhonePlaceholder.Text != null && ValidService.simpleNumberCheck.IsMatch(PhonePlaceholder.Text))
            {
                if (NamePlaceholder.Text != null && ValidService.nameCheck.IsMatch(NamePlaceholder.Text))
                {
                    (HttpStatusCode Status, Client Client) client = (HttpStatusCode.NotFound, null);
                    if (Client == null)
                        client = ClientService.NewClient(NamePlaceholder.Text, ValidService.ClearPhone(PhonePlaceholder.Text), "{car:auto}", UserData.Token);
                    else
                    {
                        client = ClientService.ChangeClientData(Client.Id, UserData.Token, NamePlaceholder.Text, PhonePlaceholder.Text, Client.CarInformation);
                        isUpdate = true;
                    }                            
                    switch (client.Status)
                    {
                        case System.Net.HttpStatusCode.OK:
                            if (parentPage != null)
                                parentPage.CurrentClient = client.Client;
                            // Передает клиента в форму регистрации заказа.
                            if (сlientsPage != null && isUpdate)
                            {                              
                                var updatedClient1 = сlientsPage.Clients.FirstOrDefault(c => c.Id == client.Client.Id);
                                updatedClient1.Name = client.Client.Name;
                                updatedClient1.Phone = client.Client.Phone;

                                var updatedClient2 = UserData.Clients.FirstOrDefault(c => c.Id == client.Client.Id);
                                updatedClient2.Name = client.Client.Name;
                                updatedClient2.Phone = client.Client.Phone;
                            }
                            else
                            {
                                if (сlientsPage != null)
                                    сlientsPage.Clients.Add(client.Client);
                                UserData.Clients.Add(client.Client);
                            }                                
                            DBService.AddOrRewriteClient(client.Client);                            
                            await Navigation.PopModalAsync();
                            
                            if (searchOrCreatePage != null)
                                searchOrCreatePage.ClosePage();
                            // Закрывает страницу поиска клиентов.
                                break;
                        case System.Net.HttpStatusCode.Created:
                            if (parentPage != null)
                                parentPage.CurrentClient = client.Client;
                            // Передает клиента в форму регистрации заказа.
                            if (сlientsPage != null && isUpdate)
                            {
                                var updatedClient1 = сlientsPage.Clients.FirstOrDefault(c => c.Id == client.Client.Id);
                                updatedClient1.Name = client.Client.Name;
                                updatedClient1.Phone = client.Client.Phone;
                                
                                var updatedClient2 = UserData.Clients.FirstOrDefault(c => c.Id == client.Client.Id);
                                updatedClient2.Name = client.Client.Name;
                                updatedClient2.Phone = client.Client.Phone;
                            }
                            else
                                UserData.Clients.Add(client.Client);
                            DBService.AddOrRewriteClient(client.Client);
                            await Navigation.PopModalAsync();

                            if (searchOrCreatePage != null)
                                searchOrCreatePage.ClosePage();
                            // Закрывает страницу поиска клиентов.
                            break;
                        default:
                            await DisplayAlert("Ошибка авторизации", $"{client.Status}", "ОK");
                            break;
                    }
                }
                else await DisplayAlert("Ошибка", $"Некорректный ввод имени", "ОK");
            }
            else await DisplayAlert("Ошибка", $"Некорректный ввод номера, номер должен начинаться с +7", "ОK");
        }
        private void NamePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}