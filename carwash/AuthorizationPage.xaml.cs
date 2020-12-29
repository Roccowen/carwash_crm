using System;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Models;

namespace carwash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorizationPage : ContentPage
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }
        public void AuthorizationClicked(object sender, EventArgs e)
        {
            if (Test.useApi)
            {
                if (numberCheck.IsMatch(NumberPlaceholder.Text) && PasswordPlaceholder.Text != null)
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                            new KeyValuePair<string, string>("phone", ClearPhone(NumberPlaceholder.Text)),
                            new KeyValuePair<string, string>("password", PasswordPlaceholder.Text),
                        });
                    var response = AppData.AppHttpClient.PostAsync(@"/api/login", content);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        AppData.Token = JsonSerializer.Deserialize<RegistrationAnswer>(response.Result.Content.ReadAsStringAsync().Result).Data["token"];
                        App.Current.Properties.Add("phone", ClearPhone(NumberPlaceholder.Text));
                        App.Current.Properties.Add("password", PasswordPlaceholder.Text);
                        Navigation.PopModalAsync();
                    }
                    else
                        ResultLabel.Text = "Неверный логин или пароль";
                }
            }
            if (Test.useLocal)
            {
                if (numberCheck.IsMatch(NumberPlaceholder.Text) && PasswordPlaceholder.Text != null)
                {
                    if (new Random().Next(0, 10) > 5)
                    {
                        App.Current.Properties.Add("phone", ClearPhone(NumberPlaceholder.Text));
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
        }
        private static Regex numberCheck = new Regex(@"(8[0-9]{10})|(\+7[0-9]{10})");
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
        private string ClearPhone(string phone)
        {
            if (phone.ToCharArray()[0] == '+')
                return phone.Replace("+7", "");
            if (phone.ToCharArray()[0] == '8')
                return phone.Replace("8", "");
            return "";
        }
    }
}