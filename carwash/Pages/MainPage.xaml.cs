using carwash.Data;

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
            currentUserGreeting = $"Добро пожаловать, {UserData.Name}!";
            this.BindingContext = this;
        }
    }
}