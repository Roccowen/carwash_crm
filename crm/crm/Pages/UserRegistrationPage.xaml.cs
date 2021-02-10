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
using carwash.Services;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]   
    public partial class UserRegistrationPage : ContentPage
    {
        public UserRegistrationPage()
        {
            InitializeComponent();
        }
        private async void Registration(object sender, EventArgs e)
        {
            if (NumberPlaceholder.Text!= null && ValidService.numberCheck.IsMatch(NumberPlaceholder.Text))
            {
                if (PasswordPlaceholder.Text != null && ValidService.passwordCheck.IsMatch(PasswordPlaceholder.Text))
                {
                    if (PasswordCPlaceholder.Text != null && PasswordPlaceholder.Text == PasswordCPlaceholder.Text)
                    {
                        if (NamePlaceholder.Text != null && ValidService.nameCheck.IsMatch(NamePlaceholder.Text))
                        {
                            var answer = UserService.Registration(
                                ValidService.ClearPhone(NumberPlaceholder.Text),
                                PasswordPlaceholder.Text,
                                PasswordCPlaceholder.Text,
                                NamePlaceholder.Text);
                            switch (answer.Status)
                            {
                                case System.Net.HttpStatusCode.OK:
                                    await Navigation.PopModalAsync();
                                    break;
                                case System.Net.HttpStatusCode.InternalServerError:
                                    await DisplayAlert("Ошибка регистрации", "Данный номер уже занят", "ОК");
                                    break;
                                default:
                                    await DisplayAlert("Ошибка регистрации", $"{answer.Status}", "ОК");
                                    break;
                            }
                        }
                        else await DisplayAlert("Ошибка", $"Некорректный ввод имени", "ОK");
                    }
                    else await DisplayAlert("Ошибка", $"Пароли должны совпадать", "ОK");
                }
                else await DisplayAlert("Ошибка", $"Пароль должен содержать шесть символов", "ОK");
            }
            else await DisplayAlert("Ошибка", $"Некорректный ввод номера", "ОK");               
        }
        private async void toBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }   
        private void NumberPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void PasswordPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void PasswordCPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
        private void NamePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}