using carwash.Data;
using carwash.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace carwash
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

        private void AddNewEmploeeButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new WorkerRegistrationPage());
        }
        private async void LeaveButton_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Подтвердить действие", "Вы действительно хотите выйти из аккаунта?", "Да", "Нет");
            if (result)
            {
                CurrentUserData.ClearData();
                await Navigation.PushModalAsync(new AuthorizationPage());
            }
        }

        private void AddNewClietnButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ClientRegistrationPage());
        }
    }
}