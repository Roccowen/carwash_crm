using carwash.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public string currentUserGreeting { get; set; }
        public MainPage()
        {
            InitializeComponent();
            currentUserGreeting = $"Добро пожаловать, {CurrentUserData.Name}!";
            this.BindingContext = this;
        }
    }
}