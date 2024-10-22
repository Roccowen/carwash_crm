﻿using carwash.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace carwash.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class СlientsPage : ContentPage
    {
        public List<Client> AllClients { get; set; }
        public ObservableCollection<Client> ClientsUI { get; set; }
        public СlientsPage()
        {
            InitializeComponent();
        }

        private void ClientsSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ClientsSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {

        }
    }
}