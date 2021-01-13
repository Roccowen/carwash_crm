using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Models;
using carwash.Data;
using carwash.Services;
using RestSharp;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkerRegistrationPage : ContentPage
    {
        private ErrorService errorService;
        public WorkerRegistrationPage()
        {
            InitializeComponent();
            errorService = new ErrorService(ResultLabel);
        }
        private void Registration(object sender, EventArgs e)
        {
            if (NamePlaceholder.Text != null && NamePlaceholder.Text != "")
            {
                var workerAnswer = WorkerService.NewWorker(CurrentUserData.Token, NamePlaceholder.Text);
                switch (workerAnswer.Status)
                {
                    case HttpStatusCode.OK:
                        if (workerAnswer.Worker != null)
                        {
                            errorService.ClearErrors();
                            Navigation.PopModalAsync();
                        }
                        break;
                    default:
                        errorService.AddError(Errors.ConnectionProblem);
                        break;
                };
            }         
        }
        private void NamePlaceholder_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
                errorService.AddError(Errors.NameMustNotBeEmpty);
            else
                errorService.DelError(Errors.NameMustNotBeEmpty);
        }
        private void toBack(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}