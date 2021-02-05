using carwash.Data;
using carwash.Pages;
using carwash.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public static int boxCount;
        public SettingsPage()
        {
            InitializeComponent();
        }     
        private async void AddNewEmploeeButton_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushModalAsync(new WorkerRegistrationPage());
        }
        private async void LeaveButton_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Подтвердить действие", "Вы действительно хотите выйти из аккаунта?", "Да", "Нет");
            if (result)
            {               
                DBService.DropData();
                CurrentUserData.ClearData();
                ApplicationData.ClientsCount = 0; //only debug
                ApplicationData.OrdersCount = 0;
                ApplicationData.WorkersCount = 0;
                await Navigation.PopModalAsync();
            }
        }
        private async void AddNewClietnButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ClientRegistrationPage(null));
        }
    }
}