using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace talsystemer.Classes
{
    public class SubnetCalculator
    {
        public string IpAddress { get; set; }
        public int SubnetMask { get; set; }

        public SubnetCalculator(string ipAddress, int subnetMask)
        {
            IpAddress = ipAddress;
            SubnetMask = subnetMask;
        }

        public string GetNetworkAddress()
        {
            var ip = IPAddress.Parse(IpAddress);
            var ipBytes = ip.GetAddressBytes();
            var maskBytes = GetSubnetMaskBytes();
            var networkAddressBytes = new byte[ipBytes.Length];

            for (int i = 0; i < ipBytes.Length; i++)
            {
                networkAddressBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }

            return new IPAddress(networkAddressBytes).ToString();
        }

        public string GetBroadcastAddress()
        {
            var ip = IPAddress.Parse(IpAddress);
            var ipBytes = ip.GetAddressBytes();
            var maskBytes = GetSubnetMaskBytes();
            var broadcastAddressBytes = new byte[ipBytes.Length];

            for (int i = 0; i < ipBytes.Length; i++)
            {
                broadcastAddressBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]);
            }

            return new IPAddress(broadcastAddressBytes).ToString();
        }

        public string GetFirstHostAddress()
        {
            var networkAddress = IPAddress.Parse(GetNetworkAddress());
            var addressBytes = networkAddress.GetAddressBytes();
            addressBytes[addressBytes.Length - 1]++;
            return new IPAddress(addressBytes).ToString();
        }

        public string GetLastHostAddress()
        {
            var broadcastAddress = IPAddress.Parse(GetBroadcastAddress());
            var addressBytes = broadcastAddress.GetAddressBytes();
            addressBytes[addressBytes.Length - 1]--;
            return new IPAddress(addressBytes).ToString();
        }

        public int GetNumberOfHosts()
        {
            int numberOfHostBits = 32 - SubnetMask;
            return (int)Math.Pow(2, numberOfHostBits) - 2; // Minus 2 for network and broadcast address
        }

        private byte[] GetSubnetMaskBytes()
        {
            var mask = new byte[4];
            for (int i = 0; i < SubnetMask; i++)
            {
                mask[i / 8] |= (byte)(1 << (7 - (i % 8)));
            }
            return mask;
        }
    }
}
