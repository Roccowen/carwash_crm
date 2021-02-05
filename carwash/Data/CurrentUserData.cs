using carwash.Models;
using System;
using Xamarin.Essentials;
using System.Collections.ObjectModel;

namespace carwash.Data
{
    public static class CurrentUserData
    {
        public static string Token
        {
            get
            {                
                try
                {
                    return SecureStorage.GetAsync("CurrentUserToken").Result;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"@Error {ex.Message}");
                    object token = "";
                    if (App.Current.Properties.TryGetValue("CurrentUserToken", out token))
                        return (string)token;
                    else
                    {
                        App.Current.Properties.Add("CurrentUserToken", (string)token);
                        return (string)token;
                    }
                }
            }
            set
            {
                try
                {
                    SecureStorage.SetAsync("CurrentUserToken", value);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"@Error {ex.Message}");
                    App.Current.Properties["CurrentUserToken"] = value;
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
        public static string Settings
        {
            get
            {
                object settings = "";
                if (App.Current.Properties.TryGetValue("CurrentUserSettings", out settings))
                    return (string)settings;
                else
                {
                    App.Current.Properties.Add("CurrentUserSettings", (string)settings);
                    return (string)settings;
                }
            }
            set
            {
                App.Current.Properties["CurrentUserSettings"] = value;
            }
        }
        public static void ClearData()
        {
            App.Current.Properties.Clear();
            SecureStorage.RemoveAll();
        }
        public static void NewUserData(User user)
        {
            CurrentUserData.Name = user.Name;
            CurrentUserData.Id = user.Id;
            CurrentUserData.MainUserId = user.MainUserId;            
            CurrentUserData.Phone = user.Phone;
            CurrentUserData.Settings = user.Settings;
            CurrentUserData.Email = user.Email;
        }

    }
}
