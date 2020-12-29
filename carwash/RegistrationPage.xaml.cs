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
        public RegistrationPage()
        {
            InitializeComponent();
            errorSet = new SortedSet<string>();
        }

        private void Registration(object sender, EventArgs e)
        {
            if (numberCheck.IsMatch(NumberPlaceholder.Text) && 
                passwordCheck.IsMatch(NumberPlaceholder.Text) && 
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
                        DelError("Проблемы с соединением");
                        Navigation.PopModalAsync();
                    }
                    else
                        AddError("Проблемы с соединением");

                }
                if (Test.useLocal)                
                    Navigation.PopModalAsync();                
            }
        }
        private void toBack(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
        
        private static Regex numberCheck = new Regex(@"(8[0-9]{10})|(\+7[0-9]{10})");
        private static Regex passwordCheck = new Regex(@"[\w\d]{6,}");
        private static SortedSet<string> errorSet;
        private void AddError(string error)
        {
            if (error != "")
            {
                errorSet.Add(error);
                ResultLabel.Text = errorSet.First();
            }          
        }
        private void DelError(string error)
        {
            errorSet.Remove(error);
            try
            {
                ResultLabel.Text = errorSet.First();
            }
            catch
            {
                ResultLabel.Text = "";
            }
        }
        private void NumberPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!numberCheck.IsMatch(e.NewTextValue))
                AddError("Номер должен начинаться с +7 или 8");
            else
                DelError("Номер должен начинаться с +7 или 8");
        }
        private void PasswordPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!passwordCheck.IsMatch(e.NewTextValue))
                AddError("Пароль должен содержать 6 символов");
            else
                DelError("Пароль должен содержать 6 символов");
        }
        private void PasswordCPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PasswordCPlaceholder.Text != PasswordPlaceholder.Text)
                AddError("Пароли должны совпадать");
            else
                DelError("Пароли должны совпадать");
        }
        private void NamePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
                AddError("Имя не может быть пустым");
            else
                DelError("Имя не может быть пустым");
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