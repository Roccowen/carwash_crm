using carwash.Data;
using carwash.Services;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public static int boxCount;
        public SettingsPage()
        {
            InitializeComponent();

            boxCount = BoxCountRead();
            BoxCountLabel.Text = boxCount.ToString();
            BoxStepper.Value = (int)boxCount;
        }
        private int BoxCountRead()
        {
            object boxCount = null;
            if (!App.Current.Properties.TryGetValue("boxCount", out boxCount))
                App.Current.Properties.Add("boxCount", 0);
            return (int)App.Current.Properties["boxCount"];
        }
        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            boxCount = (int)e.NewValue;
            BoxCountLabel.Text = boxCount.ToString();
            App.Current.Properties["boxCount"] = boxCount;
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
                AppData.ClientsCount = 0; //only debug
                AppData.OrdersCount = 0;
                AppData.WorkersCount = 0;
                await Navigation.PopModalAsync();
                //(Application.Current).MainPage = new TabbedMainPage();
            }
        }
        private async void AddNewClietnButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ClientRegistrationPage());
        }
    }
}