using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Models;
using carwash.Services;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersWorkersPage : ContentPage
    {
        public List<Worker> Workers { get; set; }
        public UsersWorkersPage()
        {
            InitializeComponent();
            Workers = null;
            this.BindingContext = this;
        }

        private async void  StackLayout_Focused(object sender, FocusEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("@StackLayout_Focused");
            var WorkersTask = WorkerService.GetAllWorkersAsync(AppData.Token);
            Workers = await WorkersTask;
        }
    }
}