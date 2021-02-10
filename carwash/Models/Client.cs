using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using carwash.Interfaces;

namespace carwash.Models
{
    public class Client : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string phone;
        private string carInformation;
        private int userId;

        [Key]
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
        [JsonPropertyName("name")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        [JsonPropertyName("phone")]
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                if (phone != value)
                {
                    phone = value;
                    OnPropertyChanged("Phone");
                }
            }
        }
        [JsonPropertyName("car_information")]
        public string CarInformation
        {
            get
            {
                return carInformation;
            }
            set
            {
                if (carInformation != value)
                {
                    carInformation = value;
                    OnPropertyChanged("CarInformation");
                }
            }
        }
        [JsonPropertyName("user_id")]
        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                if (userId != value)
                {
                    userId = value;
                    OnPropertyChanged("UserId");
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
