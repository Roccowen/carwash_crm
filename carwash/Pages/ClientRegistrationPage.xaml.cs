using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Models;
using carwash.Services;
using carwash.Data;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientRegistrationPage : ContentPage
    {
        public ClientRegistrationPage()
        {
            InitializeComponent();
        }

        private void NamePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void AddNewClient_Clicked(object sender, EventArgs e)
        {            
            if (PhonePlaceholder.Text != null && 
                NamePlaceholder.Text != null &&
                PhonePlaceholder.Text != "" &&
                NamePlaceholder.Text != "" &&
                !new Regex("[0-9]").IsMatch(NamePlaceholder.Text))
            {
                var client = ClientService.NewClient(NamePlaceholder.Text, PhonePlaceholder.Text, "{car:auto}", CurrentUserData.Token);
                switch (client.Status)
                {
                    case System.Net.HttpStatusCode.OK:
                        await Navigation.PopModalAsync();
                        break;
                    case System.Net.HttpStatusCode.Created:
                        await Navigation.PopModalAsync();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}