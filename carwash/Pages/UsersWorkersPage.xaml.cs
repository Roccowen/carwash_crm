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
    public partial class UsersWorkersPage : ContentPage
    {
        public List<Worker> Workers { get; set; }
        public UsersWorkersPage()
        {
            InitializeComponent();
            
            Workers = Worker.GetWorkers(AppData.Token);
            this.BindingContext = this;
        }
    }
}