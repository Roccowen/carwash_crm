using carwash.Pages;
using carwash.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using carwash.Data;

namespace carwash
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            if (CurrentUserData.Token != "")
            {
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
                }
            }
            else
                Navigation.PushModalAsync(new AuthorizationPage());
            if (Test.withRegistration)
                Navigation.PushModalAsync(new AuthorizationPage());            
            Detail = new NavigationPage(new AboutPage());
            IsPresented = false;            
        }
        public void SettingsButtonClick(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new SettingsPage());
            IsPresented = false;
        }
        public void AboutButtonClick(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new AboutPage());
            IsPresented = false;
        }
        private void WorkersButton_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new UsersWorkersPage());
            IsPresented = false;
        }
    }
}
