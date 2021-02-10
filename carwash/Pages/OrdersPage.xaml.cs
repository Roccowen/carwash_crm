using carwash.Models;
using carwash.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersPage : ContentPage
    {
        public ObservableCollection<OrderInfo> ordersInfo { get; set; }
        public OrdersPage()
        {
            InitializeComponent();
            ordersInfo = new ObservableCollection<OrderInfo>();

            var orderInfosDB = DBService.GetSortedOrderForDateInfos();
            orderInfosDB = orderInfosDB.Where(
                i => i.OrderDateOfReservation.ToString("yyyy-MM-dd") == DateTime.Now.Date.ToString("yyyy-MM-dd"))
                .OrderBy(o => o.OrderDateOfReservation)
                .ToList();
            if (orderInfosDB.Count != 0)
            {
                foreach (var info in orderInfosDB)
                    ordersInfo.Add(info);
                this.BindingContext = this;
            }
        }
        private void OrdersDataPicker_Unfocused(object sender, FocusEventArgs e)
        {
            ordersInfo.Clear();
            var orderInfosDB = DBService.GetSortedOrderForDateInfos();
            System.Diagnostics.Debug.WriteLine($"@ orderInfosDB count - {orderInfosDB.Count}");
            orderInfosDB = orderInfosDB.Where(
                i => i.OrderDateOfReservation.ToString("yyyy-MM-dd") == OrdersDataPicker.Date.ToString("yyyy-MM-dd"))
                .OrderBy(o => o.OrderDateOfReservation)
                .ToList();
            System.Diagnostics.Debug.WriteLine($"@ orderInfosDB count after - {orderInfosDB.Count}");
            if (orderInfosDB.Count != 0)
            {
                foreach (var info in orderInfosDB)
                    ordersInfo.Add(info);
                this.BindingContext = this;
            }
        }
        private async void NewOrderButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new OrderRegistrationPage(this, OrdersDataPicker.Date));
        }
        private void OrdersDataPicker_Focused(object sender, FocusEventArgs e)
        {

        }

        private async void DeleteContexMenu_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Подтвердить действие", "Вы действительно хотите удалить заказ", "Да", "Нет");
            if (result)
            {
                var listView = sender as MenuItem;
                var selectedOrder = listView.BindingContext as OrderInfo;
                ordersInfo.Remove(selectedOrder);


                System.Diagnostics.Debug.WriteLine("@DeleteContexMenu_Clicked is suc");
            }
        }

        private void EditContexMenu_Clicked(object sender, EventArgs e)
        {
            var selectedItem = sender as MenuItem;
        }

        private void AboutContexMenu_Clicked(object sender, EventArgs e)
        {

        }

        private void OrdersList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}