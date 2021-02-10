using carwash.Models;
using carwash.Services;
using System;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace carwash.Data
{
    public static class UserData
    {
        public static string Token
        {
            get
            {
                try
                {
                    return SecureStorage.GetAsync("UserToken").Result;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"@Error {ex.Message}");
                    object token = "";
                    if (App.Current.Properties.TryGetValue("UserToken", out token))
                        return (string)token;
                    else
                    {
                        App.Current.Properties.Add("UserToken", (string)token);
                        return (string)token;
                    }
                }
            }
            set
            {
                try
                {
                    SecureStorage.SetAsync("UserToken", value);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"@Error {ex.Message}");
                    App.Current.Properties["UserToken"] = value;
                }
            }
        }
        public static int Id
        {
            get
            {
                object id = "0";
                if (App.Current.Properties.TryGetValue("CurrentUserId", out id))
                    return Convert.ToInt32(id);
                else
                {
                    App.Current.Properties.Add("CurrentUserId", "0");
                    return Convert.ToInt32(id);
                }
            }
            set
            {
                App.Current.Properties["CurrentUserId"] = value.ToString();
            }
        }
        public static string Name
        {
            get
            {
                object name = "";
                if (App.Current.Properties.TryGetValue("CurrentUserName", out name))
                    return (string)name;
                else
                {
                    App.Current.Properties.Add("CurrentUserName", (string)name);
                    return (string)name;
                }
            }
            set
            {
                App.Current.Properties["CurrentUserName"] = value;
            }
        }
        public static string Phone
        {
            get
            {
                object phone = "";
                if (App.Current.Properties.TryGetValue("CurrentUserPhone", out phone))
                    return (string)phone;
                else
                {
                    App.Current.Properties.Add("CurrentUserPhone", (string)phone);
                    return (string)phone;
                }
            }
            set
            {
                App.Current.Properties["CurrentUserPhone"] = value;
            }
        }
        public static string Email
        {
            get
            {
                object email = "";
                if (App.Current.Properties.TryGetValue("CurrentUserEmail", out email))
                    return (string)email;
                else
                {
                    App.Current.Properties.Add("CurrentUserEmail", (string)email);
                    return (string)email;
                }
            }
            set
            {
                App.Current.Properties["CurrentUserEmail"] = value;
            }
        }
        public static string MainUserId
        {
            get
            {
                object mainUserId = "";
                if (App.Current.Properties.TryGetValue("CurrentUserMainUserId", out mainUserId))
                    return (string)mainUserId;
                else
                {
                    App.Current.Properties.Add("CurrentUserMainUserId", (string)mainUserId);
                    return (string)mainUserId;
                }
            }
            set
            {
                App.Current.Properties["CurrentUserMainUserId"] = value;
            }
        }
        private static Settings settings;
        public static Settings Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = value;
            }
        }
        public static ObservableCollection<Client> Clients { get; set; }
        public static ObservableCollection<Worker> Workers { get; set; }
        public static void ClearData()
        {
            App.Current.Properties.Clear();
            SecureStorage.RemoveAll();
            Clients = null;
            Workers = null;
        }
        public static void NewUserData(User user)
        {
            UserData.Name = user.Name;
            UserData.Id = user.Id;
            UserData.MainUserId = user.MainUserId;            
            UserData.Phone = user.Phone;
            UserData.Settings = user.Settings;
            UserData.Email = user.Email;
            
            Clients = new ObservableCollection<Client>();
            Workers = new ObservableCollection<Worker>();
        }
        public static void AddWorkers(List<Worker> workers)
        {
            workers.OrderBy(w => w.Name).ToList().ForEach(w => Workers.Add(w));
        }
        public static void AddClients(List<Client> clients)
        {
            var cl = clients.OrderBy(c => c.Name);
            foreach (var c in cl)
            {
                Clients.Add(c);
            }
        }
    }
}
