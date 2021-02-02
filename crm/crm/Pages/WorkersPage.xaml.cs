using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Models;
using carwash.Services;
using carwash.Data;

namespace crm.Pages
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

        private void  StackLayout_Focused(object sender, FocusEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("@StackLayout_Focused");
        }
    }
}