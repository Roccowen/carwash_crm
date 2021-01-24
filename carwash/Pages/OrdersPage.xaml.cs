﻿using System;
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
using System.Threading;

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

            var orderInfosDB = DBService.GetOrderInfos();
            orderInfosDB = orderInfosDB.Where(
                i => i.OrderDateOfReservation.ToString("yyyy-MM-dd") == DateTime.Now.Date.ToString("yyyy-MM-dd"))
                .ToList();
            if (orderInfosDB.Count != 0)
            {
                OrdersIsNullLabel.IsVisible = false;
                foreach (var info in orderInfosDB)
                    ordersInfo.Add(info);
                this.BindingContext = this;
            }
            else
                OrdersIsNullLabel.IsVisible = true;
        }
        private void OrdersDataPicker_Unfocused(object sender, FocusEventArgs e)
        {
            ordersInfo.Clear();
            var orderInfosDB = DBService.GetOrderInfos();
            System.Diagnostics.Debug.WriteLine($"@ orderInfosDB count - {orderInfosDB.Count}");
            orderInfosDB = orderInfosDB.Where(
                i => i.OrderDateOfReservation.ToString("yyyy-MM-dd") == OrdersDataPicker.Date.ToString("yyyy-MM-dd"))
                .ToList();
            System.Diagnostics.Debug.WriteLine($"@ orderInfosDB count after - {orderInfosDB.Count}");
            if (orderInfosDB.Count != 0)
            {
                OrdersIsNullLabel.IsVisible = false;
                foreach (var info in orderInfosDB)
                    ordersInfo.Add(info);
                this.BindingContext = this;
            }
            else
            {
                OrdersIsNullLabel.IsVisible = true;
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
    }
}