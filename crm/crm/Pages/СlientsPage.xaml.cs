using carwash.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using carwash.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class СlientsPage : ContentPage
    {
        public List<Client> AllClients { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public СlientsPage()
        {
            InitializeComponent();
            Clients = new ObservableCollection<Client>();
            AllClients = DBService.GetClients();
            foreach (var c in AllClients)
                Clients.Add(c);

            this.BindingContext = this;
        }

        private void ClientsSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ClientsSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {

        }
    }
}