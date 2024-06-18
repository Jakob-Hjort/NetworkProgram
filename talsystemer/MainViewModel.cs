using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;

namespace talsystemer
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _ipAddress;
        private ObservableCollection<string> _subnetOptions;
        private string _networkClass;

        private int _firstOctet;
        private int _secondOctet;
        private int _thirdOctet;
        private int _fourthOctet;

        private string _binFirstOctet;
        private string _binSecondOctet;
        private string _binThirdOctet;
        private string _binFourthOctet;

        private string _ipClass;
        private string _isValid;

        private bool _isDecimalInput;

        public int FirstOctet
        {
            get { return _firstOctet; }
            set
            {
                if (_firstOctet != value)
                {
                    _firstOctet = value;
                    OnPropertyChanged(nameof(FirstOctet));
                    if (_isDecimalInput)
                    {
                        UpdateBinaries();
                    }
                    else
                    {
                        UpdateIpAddress();
                    }
                }
            }
        }

        public int SecondOctet
        {
            get { return _secondOctet; }
            set
            {
                if (_secondOctet != value)
                {
                    _secondOctet = value;
                    OnPropertyChanged(nameof(SecondOctet));
                    if (_isDecimalInput)
                    {
                        UpdateBinaries();
                    }
                    else
                    {
                        UpdateIpAddress();
                    }
                }
            }
        }

        public int ThirdOctet
        {
            get { return _thirdOctet; }
            set
            {
                if (_thirdOctet != value)
                {
                    _thirdOctet = value;
                    OnPropertyChanged(nameof(ThirdOctet));
                    if (_isDecimalInput)
                    {
                        UpdateBinaries();
                    }
                    else
                    {
                        UpdateIpAddress();
                    }
                }
            }
        }

        public int FourthOctet
        {
            get { return _fourthOctet; }
            set
            {
                if (_fourthOctet != value)
                {
                    _fourthOctet = value;
                    OnPropertyChanged(nameof(FourthOctet));
                    if (_isDecimalInput)
                    {
                        UpdateBinaries();
                    }
                    else
                    {
                        UpdateIpAddress();
                    }
                }
            }
        }

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

        public string BinFirstOctet
        {
            get { return _binFirstOctet; }
            set
            {
                _binFirstOctet = value;
                OnPropertyChanged(nameof(BinFirstOctet));
                if (!_isDecimalInput)
                {
                    UpdateDecimals();
                }
            }
        }

        public string BinSecondOctet
        {
            get { return _binSecondOctet; }
            set
            {
                _binSecondOctet = value;
                OnPropertyChanged(nameof(BinSecondOctet));
                if (!_isDecimalInput)
                {
                    UpdateDecimals();
                }
            }
        }

        public string BinThirdOctet
        {
            get { return _binThirdOctet; }
            set
            {
                _binThirdOctet = value;
                OnPropertyChanged(nameof(BinThirdOctet));
                if (!_isDecimalInput)
                {
                    UpdateDecimals();
                }
            }
        }

        public string BinFourthOctet
        {
            get { return _binFourthOctet; }
            set
            {
                _binFourthOctet = value;
                OnPropertyChanged(nameof(BinFourthOctet));
                if (!_isDecimalInput)
                {
                    UpdateDecimals();
                }
            }
        }

        public string IpClass
        {
            get { return _ipClass; }
            set
            {
                _ipClass = value;
                OnPropertyChanged(nameof(IpClass));
            }
        }

        public string IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public MainViewModel()
        {
            SubnetOptions = new ObservableCollection<string>();
            UpdateSubnetOptions();
        }

        public ICommand CalculateCommand { get { return new RelayCommand(Calculate); } }

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

        private void UpdateIpAddress()
        {
            IpAddress = $"{FirstOctet}.{SecondOctet}.{ThirdOctet}.{FourthOctet}";
            UpdateNetworkClass();
        }

        private void UpdateBinaries()
        {
            _isDecimalInput = true;
            BinFirstOctet = Convert.ToString(FirstOctet, 2);
            BinSecondOctet = Convert.ToString(SecondOctet, 2);
            BinThirdOctet = Convert.ToString(ThirdOctet, 2);
            BinFourthOctet = Convert.ToString(FourthOctet, 2);
            _isDecimalInput = false;
        }

        private void UpdateDecimals()
        {
            _isDecimalInput = false;
            if (int.TryParse(BinFirstOctet, System.Globalization.NumberStyles.Integer, null, out int first))
                FirstOctet = first;
            if (int.TryParse(BinSecondOctet, System.Globalization.NumberStyles.Integer, null, out int second))
                SecondOctet = second;
            if (int.TryParse(BinThirdOctet, System.Globalization.NumberStyles.Integer, null, out int third))
                ThirdOctet = third;
            if (int.TryParse(BinFourthOctet, System.Globalization.NumberStyles.Integer, null, out int fourth))
                FourthOctet = fourth;
            _isDecimalInput = true;
        }

        private void Calculate()
        {
            string ipAddressString = $"{FirstOctet}.{SecondOctet}.{ThirdOctet}.{FourthOctet}";

            if (IPAddress.TryParse(ipAddressString, out IPAddress ipAddress))
            {
                byte[] bytes = ipAddress.GetAddressBytes();
                byte firstOctet = bytes[0];

                if (firstOctet >= 1 && firstOctet <= 126)
                    IpClass = "Class A";
                else if (firstOctet >= 128 && firstOctet <= 191)
                    IpClass = "Class B";
                else if (firstOctet >= 192 && firstOctet <= 223)
                    IpClass = "Class C";
                else if (firstOctet >= 224 && firstOctet <= 239)
                    IpClass = "Class D (Multicast)";
                else if (firstOctet >= 240 && firstOctet <= 255)
                    IpClass = "Class E (Experimental)";
                else
                    IpClass = "Unknown";

                IsValid = "Valid IP Address";
            }
            else
            {
                IpClass = "Unknown";
                IsValid = "Invalid IP Address";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
