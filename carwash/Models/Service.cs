using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace carwash.Models
{
    public class Service : INotifyPropertyChanged
    {
        private int id;
        private string title;
        private int price;
        private int durationInMinuts;
        [JsonPropertyName("id")]
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        [JsonPropertyName("title")]
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged("Title");
                }
            }
        }
        [JsonPropertyName("price")]
        public int Price
        {
            get
            {
                return price;
            }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged("Price");
                }
            }
        }
        [JsonPropertyName("duration_minuts")]
        public int DurationInMinuts
        {
            get
            {
                return durationInMinuts;
            }
            set
            {
                if (durationInMinuts != value)
                {
                    durationInMinuts = value;
                    OnPropertyChanged("DurationInMinuts");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
