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
using Plugin.Messaging;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersPage : ContentPage
    {
        public static ObservableCollection<OrderInfo> ordersInfoOnView { get; set; }
        public OrdersPage()
        {
            InitializeComponent();
            ordersInfoOnView = new ObservableCollection<OrderInfo>();
            //ApplicationData.GlobalOrderInfos = new ObservableCollection<OrderInfo>();         
            for (int i = 0; i < 13; i++)
                //ApplicationData.GlobalOrderInfos.Add(new OrderInfo(DateTime.Today.AddHours(i + 9)));
                ordersInfoOnView.Add(new OrderInfo(DateTime.Today.AddHours(i + 9)));
            OrdersList.ItemsSource = ordersInfoOnView;
            this.BindingContext = this;
            var orderInfosDB = DBService.GetSortedOrderForDateInfos();
            
            if (orderInfosDB.Count != 0)
            {
                foreach (var info in ordersInfoOnView)
                //foreach (var info in ApplicationData.GlobalOrderInfos)
                {
                    var currentOrder = orderInfosDB.FirstOrDefault(o => o.OrderDateOfReservation == info.OrderDateOfReservation);
                    if (currentOrder != null)
                        info.FillingInfo(currentOrder);
                }                           
            }
        }
        private void OrdersDataPicker_Unfocused(object sender, FocusEventArgs e)
        {
            ordersInfoOnView.Clear();
            for (int i = 0; i < 13; i++)
                ordersInfoOnView.Add(new OrderInfo(OrdersDataPicker.Date.AddHours(i + 9)));
            var orderInfosDB = DBService.GetSortedOrderForDateInfos(OrdersDataPicker.Date);
            System.Diagnostics.Debug.WriteLine($"@orderInfosDB finded orders count - {orderInfosDB.Count}");
            
            if (orderInfosDB.Count != 0)
            {           
                foreach (var info in ordersInfoOnView)
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
                await Navigation.PushModalAsync(new OrderRegistrationPage((e.Item as OrderInfo), OrdersDataPicker.Date));
                this.BindingContext = this;
            }
            else
            {
                var action = await DisplayActionSheet("Действия", "Отмена", null, "Редактировать", "Позвонить", "Отменить");
                switch (action)
                {
                    case ("Редактировать"):
                        await Navigation.PushModalAsync(new OrderRegistrationPage((e.Item as OrderInfo), OrdersDataPicker.Date));
                        break;
                    case ("Позвонить"):
                        var phoneDialer = CrossMessaging.Current.PhoneDialer;
                        if (phoneDialer.CanMakePhoneCall)
                        {
                            phoneDialer.MakePhoneCall("+7" + (e.Item as OrderInfo).ClientPhone);
                        }
                        break;
                    case ("Отменить"):
                        bool result = await DisplayAlert("Подтвердить действие", "Вы действительно хотите отменить запись?", "Да", "Нет");
                        if (result)
                        {
                            DBService.DelOrder((e.Item as OrderInfo).OrderId);
                            OrderService.DelOrder((e.Item as OrderInfo).OrderId, UserData.Token);
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