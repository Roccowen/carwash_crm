using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using carwash.Data;
using carwash.Models;
using carwash.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System.Threading;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersPage : ContentPage
    {
        public ObservableCollection<OrderInfo> ordersInfo { get; }
        public OrdersPage()
        {
            InitializeComponent();
            ordersInfo = new ObservableCollection<OrderInfo>();
            this.BindingContext = this;

            for (int i = 0; i < 13; i++)
                ordersInfo.Add(new OrderInfo(DateTime.Today.AddHours(i + 9)));
            var orderInfosDB = DBService.GetSortedOrderForDateInfos();
            
            if (orderInfosDB.Count != 0)
            {
                foreach (var info in ordersInfo)
                {
                    var currentOrder = orderInfosDB.FirstOrDefault(o => o.OrderDateOfReservation == info.OrderDateOfReservation);
                    if (currentOrder != null)
                        info.FillingInfo(currentOrder);
                }           
                
            }
        }
        private void OrdersDataPicker_Unfocused(object sender, FocusEventArgs e)
        {
            ordersInfo.Clear();
            for (int i = 0; i < 13; i++)
                ordersInfo.Add(new OrderInfo(OrdersDataPicker.Date.AddHours(i + 9)));
            var orderInfosDB = DBService.GetSortedOrderForDateInfos(OrdersDataPicker.Date);
            System.Diagnostics.Debug.WriteLine($"@orderInfosDB finded orders count - {orderInfosDB.Count}");
            
            if (orderInfosDB.Count != 0)
            {
                System.Diagnostics.Debug.WriteLine($"@orderInfosDB finded orders count - {ordersInfo.Count}");             
                foreach (var info in ordersInfo)
                {
                    var currentOrder = orderInfosDB.FirstOrDefault(o => o.OrderDateOfReservation == info.OrderDateOfReservation);
                    if (currentOrder != null)
                        info.FillingInfo(currentOrder);
                }
            }            
        }
        private async void OrdersList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if ((e.Item as OrderInfo).IsEmpty)
            {
                await Navigation.PushModalAsync(new OrderRegistrationPage(this, (e.Item as OrderInfo), OrdersDataPicker.Date));
                this.BindingContext = this;
            }
            else
            {
                var action = await DisplayActionSheet("Действия", "Отмена", null, "Редактировать", "Отменить");
                switch (action)
                {
                    case ("Редактировать"):
                        await Navigation.PushModalAsync(new OrderRegistrationPage(this, (e.Item as OrderInfo), OrdersDataPicker.Date));
                        break;
                    case ("Отменить"):
                        bool result = await DisplayAlert("Подтвердить действие", "Вы действительно хотите отменить запись?", "Да", "Нет");
                        if (result)
                        {
                            DBService.DelOrderById((e.Item as OrderInfo).OrderId);
                            OrderService.DelOrderById((e.Item as OrderInfo).OrderId, CurrentUserData.Token);
                            (e.Item as OrderInfo).ClearInfo();
                        }                         
                        break;
                    default:
                        break;
                }
            }
        }
    }
}