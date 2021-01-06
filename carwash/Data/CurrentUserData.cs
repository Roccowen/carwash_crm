using System;

namespace carwash.Data
{
    public static class CurrentUserData
    {
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
        public static string Token
        {
            get
            {
                object token = "";
                if (App.Current.Properties.TryGetValue("CurrentUserToken", out token))
                    return (string)token;
                else
                {
                    App.Current.Properties.Add("CurrentUserToken", (string)token);
                    return (string)token;
                }
            }
            set
            {
                App.Current.Properties["CurrentUserToken"] = value;
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
        public static DateTime EmailVertifiedAt
        {
            get
            {
                object emailVertifiedAt = DateTime.MinValue.ToString();
                if (App.Current.Properties.TryGetValue("CurrentUserEmailVertifiedAt", out emailVertifiedAt))
                    return (DateTime)emailVertifiedAt;
                else
                {
                    App.Current.Properties.Add("CurrentUserEmailVertifiedAt", (string)emailVertifiedAt);
                    return (DateTime)emailVertifiedAt;
                }
            }
            set
            {
                App.Current.Properties["CurrentUserEmailVertifiedAt"] = value.ToString();
            }
        }
        public static DateTime CreatedAt
        {
            get
            {
                object createdAt = DateTime.MinValue.ToString();
                if (App.Current.Properties.TryGetValue("CurrentUserCreatedAt", out createdAt))
                    return (DateTime)createdAt;
                else
                {
                    App.Current.Properties.Add("CurrentUserCreatedAt", (string)createdAt);
                    return (DateTime)createdAt;
                }
            }
            set
            {
                App.Current.Properties["CurrentUserCreatedAt"] = value;
            }
        }
        public static DateTime UpdatedAt
        {
            get
            {
                object updatedAt = DateTime.MinValue.ToString();
                if (App.Current.Properties.TryGetValue("CurrentUserUpdatedAt", out updatedAt))
                    return (DateTime)updatedAt;
                else
                {
                    App.Current.Properties.Add("CurrentUserUpdatedAt", (string)updatedAt);
                    return (DateTime)updatedAt;
                }
            }
            set
            {
                App.Current.Properties["CurrentUserUpdatedAt"] = value;
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
    }
}
