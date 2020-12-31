using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using carwash.Models;

namespace carwash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]   
    public partial class RegistrationPage : ContentPage
    {
        private ErrorController errorController;
        public RegistrationPage()
        {
            InitializeComponent();
            errorController = new ErrorController(ResultLabel);
        }

        private void Registration(object sender, EventArgs e)
        {
            if (NumberPlaceholder.Text != null &&
                PasswordPlaceholder != null &&
                PasswordCPlaceholder.Text != null &&
                NamePlaceholder.Text != null)
            {
                if (numberCheck.IsMatch(NumberPlaceholder.Text) &&
               passwordCheck.IsMatch(PasswordPlaceholder.Text) &&
               PasswordCPlaceholder.Text == PasswordPlaceholder.Text &&
               NamePlaceholder.Text != "")
                {
                    if (Test.useApi)
                    {
                        var content = new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("phone", ClearPhone(NumberPlaceholder.Text)),
                        new KeyValuePair<string, string>("password", PasswordPlaceholder.Text),
                        new KeyValuePair<string, string>("c_password", PasswordCPlaceholder.Text),
                        new KeyValuePair<string, string>("name", NamePlaceholder.Text)
                    });
                        var response = AppData.AppHttpClient.PostAsync(@"/api/register", content);
                        if (response.Result.IsSuccessStatusCode)
                        {
                            AppData.Token = JsonSerializer.Deserialize<RegistrationAnswer>(response.Result.Content.ReadAsStringAsync().Result).Data["token"];
                            errorController.DelError("Проблемы с соединением");
                            Navigation.PopModalAsync();
                        }
                        else
                            errorController.AddError("Проблемы с соединением");
                    }
                    if (Test.useLocal)
                        Navigation.PopModalAsync();
                }
            }                      
        }
        private void toBack(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }   
        private static Regex numberCheck = new Regex(@"(8[0-9]{10})|(\+7[0-9]{10})");
        private static Regex passwordCheck = new Regex(@"[\w\d]{6,}");
        private void NumberPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!numberCheck.IsMatch(e.NewTextValue))
                errorController.AddError("Номер должен начинаться с +7 или 8");
            else
                errorController.DelError("Номер должен начинаться с +7 или 8");
        }
        private void PasswordPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!passwordCheck.IsMatch(e.NewTextValue))
                errorController.AddError("Пароль должен содержать 6 символов");
            else
                errorController.DelError("Пароль должен содержать 6 символов");
        }
        private void PasswordCPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PasswordCPlaceholder.Text != PasswordPlaceholder.Text)
                errorController.AddError("Пароли должны совпадать");
            else
                errorController.DelError("Пароли должны совпадать");
        }
        private void NamePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
                errorController.AddError("Имя не может быть пустым");
            else
                errorController.DelError("Имя не может быть пустым");
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