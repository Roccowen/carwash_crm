﻿using carwash.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace crm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public string currentUserGreeting { get; set; }
        public MainPage()
        {
            InitializeComponent();
            currentUserGreeting = $"Добро пожаловать, {UserData.Name}!";
            this.BindingContext = this;
        }
    }
}