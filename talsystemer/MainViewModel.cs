using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;

namespace talsystemer
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _ipAddress;
        private ObservableCollection<string> _subnetOptions;
        private string _networkClass;

        public string IpAddress
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value;
                OnPropertyChanged(nameof(IpAddress));
                UpdateSubnetOptions();
                UpdateNetworkClass();
            }
        }

        public ObservableCollection<string> SubnetOptions
        {
            get { return _subnetOptions; }
            set
            {
                _subnetOptions = value;
                OnPropertyChanged(nameof(SubnetOptions));
            }
        }

        public string NetworkClass
        {
            get { return _networkClass; }
            set
            {
                _networkClass = value;
                OnPropertyChanged(nameof(NetworkClass));
            }
        }

        public MainViewModel()
        {
            SubnetOptions = new ObservableCollection<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateSubnetOptions()
        {
            SubnetOptions.Clear();
            if (IPAddress.TryParse(IpAddress, out _))
            {
                for (int i = 24; i <= 30; i++)
                {
                    int hosts = (int)Math.Pow(2, 32 - i) - 2;
                    SubnetOptions.Add($"/{i} - {hosts} hosts");
                }
            }
        }

        private void UpdateNetworkClass()
        {
            if (IPAddress.TryParse(IpAddress, out IPAddress ipAddress))
            {
                byte firstOctet = ipAddress.GetAddressBytes()[0];

                if (firstOctet >= 1 && firstOctet <= 126)
                {
                    NetworkClass = "Class A";
                }
                else if (firstOctet >= 128 && firstOctet <= 191)
                {
                    NetworkClass = "Class B";
                }
                else if (firstOctet >= 192 && firstOctet <= 223)
                {
                    NetworkClass = "Class C";
                }
                else if (firstOctet >= 224 && firstOctet <= 239)
                {
                    NetworkClass = "Class D (Multicast)";
                }
                else if (firstOctet >= 240 && firstOctet <= 255)
                {
                    NetworkClass = "Class E (Experimental)";
                }
                else
                {
                    NetworkClass = "Unknown";
                }
            }
            else
            {
                NetworkClass = "Invalid IP Address";
            }
        }
    }
}
