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
    // Нет никаких действий с вебом, все на локале.
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceRegistrationPage : ContentPage
    {
        private bool isUpdate { get; set; } = false;
        private Service pickedService { get; set; } = null;
        private ServiceSearchPage parentPage { get; set; } = null;
        public ServiceRegistrationPage()
        {
            InitializeComponent();
        }
        public ServiceRegistrationPage(ServiceSearchPage _parentPage)
        {
            InitializeComponent();
            parentPage = _parentPage;
        }
        public ServiceRegistrationPage(Service _service, ServiceSearchPage _parentPage)
        {
            InitializeComponent();
            
            isUpdate = true;
            pickedService = _service;
            parentPage = _parentPage;            
            
            TitlePlaceholder.Text = pickedService.Title;
            PricePlaceholder.Text = pickedService.Price.ToString();
            TimePlaceholder.Text = pickedService.DurationInMinuts.ToString();
        }

        private void AddNewServiceButton_Clicked(object sender, EventArgs e)
        {
            int price = -1;
            int time = -1;
            if (TitlePlaceholder.Text != null && TitlePlaceholder.Text != "")
            {
                if (int.TryParse(PricePlaceholder.Text, out price) && price > 0)
                {
                    if (int.TryParse(TimePlaceholder.Text, out time) && time > 0)
                    {
                        if (isUpdate)
                        {
                            var serviceToUpdate = parentPage.ServiceCollects.FirstOrDefault(s => s.Id == pickedService.Id);
                            var serviceToUpdateInData = UserData.Settings.UserServices.FirstOrDefault(s => s.Id == pickedService.Id);
                            if (serviceToUpdate != null)
                            {
                                serviceToUpdate.Price = Convert.ToInt32(PricePlaceholder.Text);
                                serviceToUpdate.Title = TitlePlaceholder.Text;
                                serviceToUpdate.DurationInMinuts = Convert.ToInt32(TimePlaceholder.Text);
                            }
                            if (serviceToUpdateInData != null)
                            {
                                serviceToUpdateInData.Price = Convert.ToInt32(PricePlaceholder.Text);
                                serviceToUpdateInData.Title = TitlePlaceholder.Text;
                                serviceToUpdateInData.DurationInMinuts = Convert.ToInt32(TimePlaceholder.Text);
                            }
                            Navigation.PopModalAsync();
                        }
                        else
                        {
                            var newService = new Service
                            {
                                Id = UserData.Settings.UserServices.Count(),
                                DurationInMinuts = Convert.ToInt32(TimePlaceholder.Text),
                                Price = Convert.ToInt32(PricePlaceholder.Text),
                                Title = TitlePlaceholder.Text
                            };
                            if (!(parentPage is null))
                            {
                                parentPage.ServiceCollects.Add(newService);
                            }
                            UserData.Settings.UserServices.Add(newService);
                        }
                    }
                    else
                        DisplayAlert("Ошибка", "Время введено некорректно", "ОК");
                }
                else
                    DisplayAlert("Ошибка", "Цена введена некорректно", "ОК");
            }
            else
                DisplayAlert("Ошибка", "Название услуги не должно быть пустым", "ОК");
        }
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}