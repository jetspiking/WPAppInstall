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
        /// <summary>
        /// The following result codes are possible when connecting a device.
        /// </summary>
        public enum ErrorResults
        {
            NoError,
            MultipleDevices,
            ConfigError
        }

        public Int32 portMapped;
        public ErrorResults errorResult;

        /// <summary>
        /// Get the type of the phone connected.
        /// </summary>
        /// <returns>Type of the phone connected.</returns>
        public PhoneTypes? GetPhoneType()
        {
            errorResult = ErrorResults.NoError;
            uint windowsPhone8Port = NativeMethods.GetWinPhone8Port(Utils.Constants.WindowsPhonePort, out portMapped);
            if (windowsPhone8Port!=0U)
            {
                switch(windowsPhone8Port)
                {
                    case 2306021393U:
                        // Multiple Devices Connected
                        errorResult = ErrorResults.MultipleDevices;
                        return null;
                    case 2306021394U:
                        // Windows Phone 8 Config Error
                        errorResult = ErrorResults.ConfigError;
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
