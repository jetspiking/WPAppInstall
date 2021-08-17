using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPAppInstall.WindowsPhone.Hardware;

namespace WPAppInstall.WindowsPhone.Common
{
    /// <summary>
    /// This class contains a few functions to verify what kind of device is connected.
    /// </summary>

    public class PhoneType
    {
        public enum ErrorResults
        {
            NoError,
            MultipleDevices,
            ConfigError
        }

        public int _portMapped;
        public ErrorResults _errorResult;

        public PhoneTypes? GetPhoneType()
        {
            _errorResult = ErrorResults.NoError;
            uint windowsPhone8Port = NativeMethods.GetWinPhone8Port(Utils.Constants.PHONE_PORT, out _portMapped);
            if (windowsPhone8Port!=0U)
            {
                switch(windowsPhone8Port)
                {
                    case 2306021393U:
                        // Multiple Devices Connected
                        _errorResult = ErrorResults.MultipleDevices;
                        return null;
                    case 2306021394U:
                        // Windows Phone 8 Config Error
                        _errorResult = ErrorResults.ConfigError;
                        return null;
                }
            }
            else
            {
                return PhoneTypes.WindowsPhone8Or10;
            }
            return PhoneTypes.WindowsPhone7;
        }
    }
}
