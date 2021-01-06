using System;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Models;
using carwash.Services;
using carwash.Data;
using System.Net;

namespace carwash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorizationPage : ContentPage
    {
        private ErrorService errorService;
        public AuthorizationPage()
        {
            InitializeComponent();
            errorService = new ErrorService(ResultLabel);
        }
        public void AuthorizationClicked(object sender, EventArgs e)
        {
            if (NumberPlaceholder.Text != null && PasswordPlaceholder.Text != null && numberCheck.IsMatch(NumberPlaceholder.Text) && PasswordPlaceholder.Text!="")
            {
                if (Test.useApi)
                {
                    var answer = UserService.Authorization(ClearPhone(NumberPlaceholder.Text), PasswordPlaceholder.Text);
                    switch (answer.Status)
                    {
                        case System.Net.HttpStatusCode.OK:
                            if (answer.Answer != null)
                            {
                                CurrentUserData.Token = answer.Answer.Data["token"];
                                var currentUserAnswer = UserService.GetUser(CurrentUserData.Token);
                                if (currentUserAnswer.Status == HttpStatusCode.OK)
                                {
                                    CurrentUserData.Id = currentUserAnswer.User.Id;
                                    CurrentUserData.MainUserId = currentUserAnswer.User.MainUserId;
                                    CurrentUserData.Name = currentUserAnswer.User.Name;
                                    CurrentUserData.Phone = currentUserAnswer.User.Phone;
                                    CurrentUserData.Settings = currentUserAnswer.User.Settings;
                                    CurrentUserData.Email = currentUserAnswer.User.Email;
                                    CurrentUserData.CreatedAt = currentUserAnswer.User.CreatedAt;
                                    CurrentUserData.UpdatedAt = currentUserAnswer.User.UpdatedAt;
                                    CurrentUserData.EmailVertifiedAt = currentUserAnswer.User.EmailVertifiedAt;
                                    errorService.ClearErrors();
                                    Navigation.PopModalAsync();
                                }
                                else
                                    errorService.AddError(Errors.ConnectionProblem);
                            }
                            break;
                        case System.Net.HttpStatusCode.NotFound:
                            errorService.AddError(Errors.ConnectionProblem);
                            break;
                        case System.Net.HttpStatusCode.InternalServerError:
                            errorService.AddError(Errors.WrongLoginOrPassword);
                            break;
                        default:
                            break;
                    }
                }
                if (Test.useLocal)
                {
                    if (new Random().Next(0, 10) > 5)
                    {
                        App.Current.Properties.Add("Phone", ClearPhone(NumberPlaceholder.Text));
                        App.Current.Properties.Add("Password", PasswordPlaceholder.Text);
                        errorService.ClearErrors();
                        Navigation.PopModalAsync();
                    }
                    else
                        errorService.AddError(Errors.WrongLoginOrPassword);
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
            if (e.NewTextValue == "")
                errorService.AddError(Errors.PhoneMustNotBeEmpty);
            if (e.NewTextValue != "")
                errorService.DelError(Errors.PhoneMustNotBeEmpty);
            if (!numberCheck.IsMatch(e.NewTextValue))
                errorService.AddError(Errors.UncorrectPhoneFormat);
            if (numberCheck.IsMatch(e.NewTextValue))
                errorService.DelError(Errors.UncorrectPhoneFormat);
        }
        private string ClearPhone(string phone)
        {
            if (phone.ToCharArray()[0] == '+')
                return phone.Replace("+7", "");
            if (phone.ToCharArray()[0] == '8')
                return phone.Replace("8", "");
            return "";
        }
        private void PasswordPlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
                errorService.AddError(Errors.PasswordMustNotBeEmpty);
            if (e.NewTextValue != "")
                errorService.DelError(Errors.PasswordMustNotBeEmpty);
        }
    }
}