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
                System.Diagnostics.Debug.WriteLine("@Token is not empty");
                var currentUserAnswer = UserService.GetCurrentUser(CurrentUserData.Token);
                if (currentUserAnswer.Status == HttpStatusCode.OK)
                {
                    CurrentUserData.Id = currentUserAnswer.User.Id;
                    CurrentUserData.MainUserId = currentUserAnswer.User.MainUserId;
                    CurrentUserData.Name = currentUserAnswer.User.Name;
                    CurrentUserData.Phone = currentUserAnswer.User.Phone;
                    CurrentUserData.Settings = currentUserAnswer.User.Settings;
                    CurrentUserData.Email = currentUserAnswer.User.Email;
                }
                else
                    CurrentUserData.Id = -1;
            }
            if (CurrentUserData.Id == -1 || CurrentUserData.Token == "")
            {
                System.Diagnostics.Debug.WriteLine("@Token is not empty or Id is -1");
                Navigation.PushModalAsync(new AuthorizationPage());
            }
                
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
        public void OrderButtonClick(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new OrdersPage());
            IsPresented = false;
        }
        private void WorkersButton_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new UsersWorkersPage());
            IsPresented = false;
        }
    }
}
