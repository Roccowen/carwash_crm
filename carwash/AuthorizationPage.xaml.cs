using System;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace carwash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorizationPage : ContentPage
    {
        private static Regex numberCheck;
        public AuthorizationPage()
        {
            InitializeComponent();
            numberCheck = new Regex(@"(8[0-9]{10})|(\+7[0-9]{10})");
        }
        public void AuthorizationClicked(object sender, EventArgs e)
        {
            if (numberCheck.IsMatch(NumberPlaceholder.Text) && PasswordPlaceholder.Text != null)
            {
                //var response = new HttpClient().GetAsync();
                if (new Random().Next(0, 10) > 7)
                {
                    App.Current.Properties.Clear();
                    App.Current.Properties.Add("number", NumberPlaceholder.Text);
                    App.Current.Properties.Add("password", PasswordPlaceholder.Text);
                    ResultLabel.Text = "Успешная попытка авторизации";
                    Navigation.PopModalAsync();
                }
                else
                {
                    ResultLabel.Text = "Неверный логин или пароль";
                }
            }
        }
        public void ToRegistration(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new RegistrationPage());
        }
        private void NumberPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!numberCheck.IsMatch(e.NewTextValue))
                ResultLabel.Text = "Номер должен начинаться с +7 или 8";
            else
                ResultLabel.Text = "";
        }
    }
}