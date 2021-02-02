using carwash.Models;
using carwash.Services;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkersPage : ContentPage
    {
        public List<Worker> Workers { get; set; }
        public WorkersPage()
        {
            InitializeComponent();
            Workers = DBService.GetWorkers();
            this.BindingContext = this;
        }

        private void StackLayout_Focused(object sender, FocusEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("@StackLayout_Focused");
        }
    }
}