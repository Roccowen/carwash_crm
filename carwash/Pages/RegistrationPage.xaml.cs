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

namespace carwash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]   
    public partial class RegistrationPage : ContentPage
    {
        private ErrorService errorService;
        public RegistrationPage()
        {
            InitializeComponent();
            errorService = new ErrorService(ResultLabel);
        }

        private void Registration(object sender, EventArgs e)
        {
            if (NumberPlaceholder.Text != null &&
                PasswordPlaceholder != null &&
                PasswordCPlaceholder.Text != null &&
                NamePlaceholder.Text != null &&
                numberCheck.IsMatch(NumberPlaceholder.Text) &&
                passwordCheck.IsMatch(PasswordPlaceholder.Text) &&
                PasswordCPlaceholder.Text == PasswordPlaceholder.Text &&
                NamePlaceholder.Text != "")
            {
                if (Test.useApi)
                {
                    var answer = UserService.Registration(
                        ClearPhone(NumberPlaceholder.Text),
                        PasswordPlaceholder.Text,
                        PasswordCPlaceholder.Text,
                        NamePlaceholder.Text);
                    switch (answer.Status)
                    {
                        case System.Net.HttpStatusCode.OK:
                            if (answer.Answer != null)
                            {
                                errorService.ClearErrors();
                                Navigation.PopModalAsync();
                            }
                            break;
                        case System.Net.HttpStatusCode.NotFound:
                            errorService.AddError(Errors.ConnectionProblem);
                            break;
                        case System.Net.HttpStatusCode.InternalServerError:
                            errorService.AddError(Errors.ThisPhoneAlreadyTaken);
                            break;
                        default:
                            break;
                    }
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
        private void NumberPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!numberCheck.IsMatch(e.NewTextValue))
                errorService.AddError("Номер должен начинаться с +7 или 8");
            else
                errorService.DelError("Номер должен начинаться с +7 или 8");
        }
        private void PasswordPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!passwordCheck.IsMatch(e.NewTextValue))
                errorService.AddError("Пароль должен содержать 6 символов");
            else
                errorService.DelError("Пароль должен содержать 6 символов");
        }
        private void PasswordCPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PasswordCPlaceholder.Text != PasswordPlaceholder.Text)
                errorService.AddError("Пароли должны совпадать");
            else
                errorService.DelError("Пароли должны совпадать");
        }
        private void NamePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
                errorService.AddError("Имя не может быть пустым");
            else
                errorService.DelError("Имя не может быть пустым");
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