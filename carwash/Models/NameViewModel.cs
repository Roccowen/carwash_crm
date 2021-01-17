using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace carwash.Models
{
    class NameViewModel : INotifyPropertyChanged
    {
        string name;

        public event PropertyChangedEventHandler PropertyChanged;


        public string Name
        {
            set
            {
                if (name != value)
                {
                    name = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    }
                }
            }
            get
            {
                return name;
            }
        }
    }
}
