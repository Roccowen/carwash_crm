using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Models;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkerRegistrationPage : ContentPage
    {
        public WorkerRegistrationPage()
        {
            InitializeComponent();
        }
        private void Registration(object sender, EventArgs e)
        {
            if (NamePlaceholder.Text != "")
            {
                var worker = Worker.NewWorker(AppData.Token, NamePlaceholder.Text);
                if (worker != null)
                {
                    ResultLabel.Text = "";
                    Navigation.PopModalAsync();
                }
                else
                    ResultLabel.Text = "Проблемы с соединением";                
            }           
        }
        private void NamePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
                ResultLabel.Text = "Имя не может быть пустым";
            else
                ResultLabel.Text = "";
        }
        private void toBack(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}