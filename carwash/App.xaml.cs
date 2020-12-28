using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace carwash
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            TestData.InitialParams();
            AppData.AppHttpClient.BaseAddress = AppData.ApiRoute;
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
