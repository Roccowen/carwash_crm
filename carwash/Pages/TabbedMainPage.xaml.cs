
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Services;
using carwash.Data;
using carwash.Models;
using System.Collections.Generic;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedMainPage : TabbedPage
    {
        public TabbedMainPage(List<Client> clients, List<Worker> workers)
        {
            InitializeComponent();
        }
    }
}