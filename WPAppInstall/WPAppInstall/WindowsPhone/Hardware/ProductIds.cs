using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPAppInstall.Resources.Strings;
using static WPAppInstall.WindowsPhone.Hardware.VendorIds;

namespace WPAppInstall.WindowsPhone.Hardware
{
    /// <summary>
    /// Allows checking for existing Windows Phone devices.
    /// </summary>
    public static class ProductIds
    {
        /// <summary>
        /// Check if the product name is known.
        /// </summary>
        /// <param name="name">Name of the device.</param>
        /// <returns>True or false for knowing or not known the product.</returns>
        public static Boolean Contains(String name)
        {
            foreach (Devices device in Enum.GetValues(typeof(Devices)))
                if (name.ToLower().Contains(device.ToString().ToLower()))
                    return true;

            foreach (DeviceSeries deviceSeries in Enum.GetValues(typeof(DeviceSeries)))
                if (name.ToLower().Contains(deviceSeries.ToString().ToLower()))
                    return true;

            return false;
        }

        /// <summary>
        /// Verify if the device series is known.
        /// </summary>
        public enum DeviceSeries
        {
            LUMIA
        }

        /// <summary>
        /// Known Windows Phone devices.
        /// </summary>
        public enum Devices
        {
            // Windows Phone 7 Devices

            VENUE_PRO,          // DELL
            PRO_7,              // HTC
            SURROUND_7,         // HTC
            TROPHY_7,           // HTC
            MOZART_7,           // HTC
            HD_7,               // HTC
            OPTIMUS_7,          // LG
            QUANTUM,            // LG
            FOCUS,              // SAMSUNG
            OMNIA_7,            // SAMSUNG

            // Windows Phone 7.5 Devices

            ALLEGRO,            // ACER
            ONETOUCH_VIEW,      // ALCATEL
            IS12T,              // FUJITSU
            RADAR,              // HTC
            TITAN,              // HTC
            TITAN_2,            // HTC
            LUMIA_510,          // NOKIA
            LUMIA_610,          // NOKIA
            LUMIA_710,          // NOKIA
            LUMIA_800,          // NOKIA
            LUMIA_900,          // NOKIA
            FOCUS_2,            // SAMSUNG
            FOCUS_S,            // SAMSUNG
            OMNIA_M,            // SAMSUNG
            OMNIA_W,            // SAMSUNG
            ORBIT,              // ZTE
            TANIA,              // ZTE

            // Windows Phone 7.8 Devices

            LUMIA_505,          // NOKIA

            // Windows Phone 8.0 Devices

            _8S,                // HTC
            _8XT,               // HTC
            _8X,                // HTC
            ASCEND_W1,          // HUAWEI
            ASCEND_W2,          // HUAWEI
            LUMIA_520,          // NOKIA
            LUMIA_521,          // NOKIA
            LUMIA_525,          // NOKIA
            LUMIA_526,          // NOKIA
            LUMIA_620,          // NOKIA
            LUMIA_625,          // NOKIA
            LUMIA_720,          // NOKIA
            LUMIA_810,          // NOKIA
            LUMIA_820,          // NOKIA
            LUMIA_822,          // NOKIA
            LUMIA_920,          // NOKIA
            LUMIA_925,          // NOKIA
            LUMIA_928,          // NOKIA
            LUMIA_1020,         // NOKIA
            LUMIA_1320,         // NOKIA
            LUMIA_1520,         // NOKIA
            LUMIA_ICON,         // NOKIA
            ATIV_ODYSSEY,       // SAMSUNG
            ATIV_S,             // SAMSUNG
            ATIV_S_NEO,         // SAMSUNG
            ATIV_SE,            // SAMSUNG

            // Windows Phone 8.1 Devices

            LIQUID_M220,        // ACER
            NANA,               // HISENSE
            ONETOUCH_PIXI_34,   // ALCATEL
            ONETOUCH_PIXI_345,  // ALCATEL
            ONETOUCH_POP_24,    // ALCATEL
            ONETOUCH_POP_245,   // ALCATEL
            CESIUM_40,          // ARCHOS
            _5703A,             // KTOUCH
            _5705A,             // KTOUCH
            _5757A,             // KTOUCH
            E8,                 // KTOUCH
            IRIS_WIN_1,         // LAVA
            LANCET,             // LG
            ONE_M8,             // HTC
            LUMIA_430,          // NOKIA
            LUMIA_435,          // NOKIA
            LUMIA_532,          // MICROSOFT
            LUMIA_530,          // NOKIA
            LUMIA_535,          // MICROSOFT
            LUMIA_540,          // MICROSOFT
            LUMIA_630,          // NOKIA
            LUMIA_635,          // NOKIA
            LUMIA_640,          // MICROSOFT
            LUMIA_640_XL,       // MICROSOFT
            LUMIA_636,          // NOKIA
            LUMIA_638,          // NOKIA
            LUMIA_730,          // NOKIA
            LUMIA_735,          // NOKIA
            LUMIA_830,          // NOKIA
            LUMIA_930,          // NOKIA
            ADVANCE_4,          // POLAROID
            ADVANCE_5,          // POLAROID
            WIN_PHONE_47_HD,    // TREKSTOR

            // Windows 10 Mobile Devices

            LIQUID_M320,        // ACER
            LIQUID_M330,        // ACER
            JADE_PRIMO,         // ACER
            ONETOUCH_FIERCE_XL, // ALCATEL
            ONETOUCH_IDOL_4S,   // ALCATEL
            ONETOUCH_PIXI_38,   // ALCATEL
            CESIUM_50,          // ARCHOS
            LUMIA_550,          // MICROSOFT
            LUMIA_650,          // MICROSOFT
            LUMIA_950,          // MICROSOFT
            LUMIA_950_XL,       // MICROSOFT
            ELITE_X3,           // HP
            WIN_PHONE_50        // TREKSTOR
        }

        /// <summary>
        /// Identifier to recognize Windows Phone 7 devices.
        /// </summary>
        private static readonly Dictionary<String, String> productIds = new Dictionary<String, String>()
        {
            { "04EC", AppStrings.APP_WINDOWS_PHONE_7_DEVICE },
        };

        /// <summary>
        /// Try to read more information with the product id.
        /// </summary>
        /// <param name="pid">Identifier for the product.</param>
        /// <returns>String that contains more information about the product.</returns>
        public static String GetPIDAddition(String pid)
        {
            String value;
            if (productIds.TryGetValue(pid, out value))
                return value;
            return null;
        }

    }
}
