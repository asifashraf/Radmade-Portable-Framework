using System;
using System.Net;

namespace WebAreas.Lib.WebAuth
{
    public class IPHelper
    {
        public int IPToInt(string ipAddress)
        {
            return BitConverter.ToInt32(IPAddress.Parse(ipAddress).GetAddressBytes(), 0);
        }

        public string IntToIP(int ipAddress)
        {
            return new IPAddress(BitConverter.GetBytes(ipAddress)).ToString();

        }
    }
}
