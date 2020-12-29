using carwash.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace carwash
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            object name = null;
            object password = null;
            if (App.Current.Properties.TryGetValue("name", out name) && App.Current.Properties.TryGetValue("password", out password))
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
