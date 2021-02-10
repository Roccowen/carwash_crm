using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using carwash.Models;
using System.Collections.ObjectModel;
using carwash.Data;
using System.Text.RegularExpressions;
using carwash.Services;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceSearchPage : ContentPage
    {
        public ObservableCollection<Service> ServiceCollects { get; set; }
        public ServiceSearchPage()
        {           
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"ServiceSearchPage() Init UserServices.Count-{UserData.Settings.UserServices.Count}");
            ServiceCollects = new ObservableCollection<Service>();
            UserData.Settings.UserServices.ForEach(s => ServiceCollects.Add(s));
            this.BindingContext = this;
        }

        private async void ServiceList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var action = await DisplayActionSheet("Действия", "Отмена", null, "Редактировать", "Удалить");
            var pickedService = e.Item as Service;
            switch (action)
            {
                case ("Редактировать"):
                    await Navigation.PushModalAsync(new ServiceRegistrationPage(pickedService, this));
                    break;
                case ("Удалить"):
                    bool result = await DisplayAlert("Подтвердить действие", "Вы действительно хотите удалить услугу? Все записи с этой услугой также будут удалены.", "Да", "Нет");
                    if (result)
                    {
                        ServiceCollects.Remove(pickedService);
                        UserData.Settings.UserServices.Remove(pickedService);
                    }
                    break;
                default:
                    break;
            }
        }

        private void ServiceFoundEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != e.OldTextValue)
            {
                Regex regex = new Regex(e.NewTextValue.ToLower());
                ServiceCollects.Clear();
                var founded = UserData.Settings.UserServices.Where(c => regex.IsMatch(c.Title.ToLower())).OrderBy(c => c.Title).ToList();
                if (founded.Count() > 0)
                    foreach (var c in founded)
                        ServiceCollects.Add(c);
                else
                {
                    founded = ServiceCollects.Where(c => regex.IsMatch(c.Price.ToString())).OrderBy(c => c.Price).ToList();
                    if (founded.Count() > 0)
                        foreach (var c in founded)
                            ServiceCollects.Add(c);
                }
            }
        }

        private async void AddServiceButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ServiceRegistrationPage(this));
        }
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}