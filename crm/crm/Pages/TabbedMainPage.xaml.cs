using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using carwash.Data;
using carwash.Models;
using carwash.Services;
using carwash.Pages;
using System.Net;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedMainPage : TabbedPage
    {
        public TabbedMainPage()
        {
            InitializeComponent();           
        }
        protected override bool OnBackButtonPressed()
        {
            // Do your magic here
            return false;
        }
    }
}