using carwash.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Models;
using carwash.Pages;

namespace carwash
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Test.InitialParams();

            MainPage = new NavigationPage(new TabbedMainPage());
            //MainPage = new NavigationPage(new TabbedMainPage());
            //MainPage = new NavigationPage(new MainPageOld());
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
