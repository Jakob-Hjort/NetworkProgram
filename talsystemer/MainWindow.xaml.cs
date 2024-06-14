using System;
using System.Windows;
using System.Windows.Controls;
using talsystemer.Classes;

namespace talsystemer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedItem is TabItem selectedTab)
            {
                switch (selectedTab.Name)
                {
                    case "TemperatureCalculate":
                        ClearTemperatureCalculator();
                        break;

                    case "Number_System_Converter":
                        break;

                    case "Subnet_calculater":
                        break;
                }
            }
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtCelsius.Text, out double C))
            {
                var F = C * (9d / 5d) + 32;
                txtFahrenheit.Text = F.ToString("0.0");
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearTemperatureCalculator();
            }
        }

        private void btnUdregn_Click(object sender, RoutedEventArgs e)
        {
            if (cmbInputType.SelectedItem is ComboBoxItem selectedItem)
            {
                string inputType = selectedItem.Content.ToString();
                string input = txtInput.Text;

                try
                {
                    int decimalNumber = 0;

                    if (inputType == "Decimal")
                    {
                        if (int.TryParse(input, out decimalNumber))
                        {
                            txtDecimal.Text = decimalNumber.ToString();
                        }
                        else
                        {
                            throw new FormatException("Invalid decimal number");
                        }
                    }
                    else if (inputType == "Binær")
                    {
                        if (IsBinary(input))
                        {
                            decimalNumber = Convert.ToInt32(input, 2);
                            txtDecimal.Text = decimalNumber.ToString();
                        }
                        else
                        {
                            throw new FormatException("Invalid binary number");
                        }
                    }
                    else if (inputType == "Hexadecimal")
                    {
                        if (IsHexadecimal(input))
                        {
                            decimalNumber = Convert.ToInt32(input, 16);
                            txtDecimal.Text = decimalNumber.ToString();
                        }
                        else
                        {
                            throw new FormatException("Invalid hexadecimal number");
                        }
                    }

                    // Konverter til binær
                    txtBinaer.Text = Convert.ToString(decimalNumber, 2);

                    // Konverter til hexadecimal
                    txtHexadecimal.Text = Convert.ToString(decimalNumber, 16).ToUpper();

                    // Konverter til oktal
                    txtOktal.Text = Convert.ToString(decimalNumber, 8);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ClearNumberSystemConverter();
                }
            }
        }

        private bool IsBinary(string input)
        {
            foreach (char c in input)
            {
                if (c != '0' && c != '1')
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsHexadecimal(string input)
        {
            foreach (char c in input)
            {
                if (!Uri.IsHexDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private void ClearTemperatureCalculator()
        {
            txtCelsius.Text = string.Empty;
            txtFahrenheit.Text = string.Empty;
        }

        private void ClearNumberSystemConverter()
        {
            txtInput.Text = string.Empty;
            txtDecimal.Text = string.Empty;
            txtBinaer.Text = string.Empty;
            txtHexadecimal.Text = string.Empty;
            txtOktal.Text = string.Empty;
            cmbInputType.SelectedIndex = -1; // Clear selection
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var viewModel = DataContext as MainViewModel;
                string ipAddress = viewModel.IpAddress;
                if (Subnet.SelectedItem is string selectedSubnet)
                {
                    int subnetMask = int.Parse(selectedSubnet.Split(' ')[0].TrimStart('/'));

                    var calculator = new SubnetCalculator(ipAddress, subnetMask);

                    NetworkAddressTextBox.Text = calculator.GetNetworkAddress();
                    BroadcastAddressTextBox.Text = calculator.GetBroadcastAddress();
                    FirstHostAddressTextBox.Text = calculator.GetFirstHostAddress();
                    LastHostAddressTextBox.Text = calculator.GetLastHostAddress();
                    NumberOfHostsTextBox.Text = calculator.GetNumberOfHosts().ToString();
                }
                else
                {
                    MessageBox.Show("Please select a valid subnet mask.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
