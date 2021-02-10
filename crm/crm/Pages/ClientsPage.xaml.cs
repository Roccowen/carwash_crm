using carwash.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using carwash.Services;
using carwash.Data;

using Plugin.Messaging;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class СlientsPage : ContentPage
    {
        public ObservableCollection<Client> Clients { get; set; }
        public СlientsPage()
        {
            InitializeComponent();

            Clients = new ObservableCollection<Client>();
            foreach (var c in DBService.GetClients())
                Clients.Add(c);
            this.BindingContext = this;

        }
        private void ClientsSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex(ClientsSearchBar.Text.ToLower());
            Clients.Clear();
            var founded = UserData.Clients.ToList().Where(c => regex.IsMatch(c.Name.ToLower())).OrderBy(c => c.Name).ToList();
            System.Diagnostics.Debug.WriteLine($"founded {founded.Count} on {ClientsSearchBar.Text}");
            if (founded.Count() > 0)
                foreach (var c in founded)
                    Clients.Add(c);
            else
            {
                founded = UserData.Clients.Where(c => regex.IsMatch(c.Phone.ToLower())).OrderBy(c => c.Phone).ToList();
                if (founded.Count() > 0)
                    foreach (var c in founded)
                        Clients.Add(c);
            }
        }
        private async void AddNewClietnButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ClientRegistrationPage(this));
        }
        private void ClientsSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {

        }

        private async void ClientsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var action = await DisplayActionSheet("Действия", "Отмена", null, "Редактировать", "Позвонить", "Удалить");
            switch (action)
            {
                case ("Редактировать"):
                    await Navigation.PushModalAsync(new ClientRegistrationPage((e.Item as Client), this));
                    break;
                case ("Позвонить"):
                    var phoneDialer = CrossMessaging.Current.PhoneDialer;
                    if (phoneDialer.CanMakePhoneCall)
                        phoneDialer.MakePhoneCall("+7" + (e.Item as Client).Phone);
                    break;
                //case ("Записать"):
                //    await Navigation.PushModalAsync(new ClientRegistrationPage((e.Item as Client), this));
                //    break;
                case ("Удалить"):
                    bool result = await DisplayAlert("Подтвердить действие", "Вы действительно хотите удалить клиента? Все записи с этим клиентом также будут удалены.", "Да", "Нет");
                    if (result)
                    {
                        UserData.Clients.Remove((e.Item as Client));
                        Clients.Remove((e.Item as Client));
                        DBService.DelClient((e.Item as Client));
                        DBService.DelClientOrders((e.Item as Client));
                        var ordersToDel = OrdersPage.ordersInfoOnView//(Application.Current.Resources["ordersInfoOnView"] as ObservableCollection<OrderInfo>)
                            .Where(o => o.ClientId == (e.Item as Client).Id);
                        foreach (OrderInfo o in ordersToDel)
                            o.ClearInfo();
                        ClientService.DelClient((e.Item as Client).Id, UserData.Token);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}