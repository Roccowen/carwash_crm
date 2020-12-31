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
        private ErrorController errorController;
        public WorkerRegistrationPage()
        {
            InitializeComponent();
            errorController = new ErrorController(ResultLabel);
        }
        private void Registration(object sender, EventArgs e)
        {
            if (NamePlaceholder.Text != null)
            {
                if (NamePlaceholder.Text != "")
                {
                    var worker = Worker.NewWorker(AppData.Token, NamePlaceholder.Text);
                    if (worker != null)
                    {
                        errorController.DelError("Проблемы с соединением");
                        Navigation.PopModalAsync();
                    }
                    else
                        errorController.AddError("Проблемы с соединением");
                }
            }         
        }
        private void NamePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
                errorController.AddError("Имя не может быть пустым");
            else
                errorController.DelError("Имя не может быть пустым");
        }
        private void toBack(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}