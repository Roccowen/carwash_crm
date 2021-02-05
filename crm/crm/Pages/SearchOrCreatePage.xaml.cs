using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using carwash.Models;
using carwash.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchOrCreatePage : ContentPage
    {
        public ObservableCollection<Client> ClientsCollect { get; set; }
        private List<Client> allClients { get; set; }
        private OrderRegistrationPage _parentPage { get; set; }
        public SearchOrCreatePage(OrderRegistrationPage parentPage)
        {
            InitializeComponent();
            ClientsCollect = new ObservableCollection<Client>();

            _parentPage = parentPage;
            allClients = DBService.GetClients().OrderBy(c => c.Name).ToList();
            foreach (var c in allClients)
                ClientsCollect.Add(c);

            this.BindingContext = this;
        }

        private void ClientFoundEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != e.OldTextValue)
            {
                Regex regex = new Regex(e.NewTextValue.ToLower());
                ClientsCollect.Clear();
                var founded = allClients.Where(c => regex.IsMatch(c.Name.ToLower())).OrderBy(c => c.Name).ToList();
                if (founded.Count() > 0)
                {
                    foreach (var c in founded)
                        ClientsCollect.Add(c);
                }
                else
                {
                    founded = allClients.Where(c => regex.IsMatch(c.Phone.ToLower())).OrderBy(c => c.Phone).ToList();
                    if (founded.Count() > 0)
                    {
                        foreach (var c in founded)
                            ClientsCollect.Add(c);
                    }
                }
            }            
        }
        private async void AddClientButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ClientRegistrationPage(ClientFoundEntry.Text, _parentPage, this));
        }
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void ClientsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            _parentPage.CurrentClient = e.Item as Client;
            await Navigation.PopModalAsync();
        }
        public async void ClosePage()
        {
            await Navigation.PopModalAsync();
        }
    }
}