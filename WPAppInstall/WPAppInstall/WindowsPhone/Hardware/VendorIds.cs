using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPAppInstall.WindowsPhone.Hardware
{
    /// <summary>
    /// Check for known Windows Phone vendors.
    /// </summary>
    public static class VendorIds
    {
        /// <summary>
        /// List of companies that are registered vendors for Windows Phones.
        /// </summary>
        public enum Vendors
        {
            Nokia,
            Htc,
            Lg,
            Dell,
            Samsung,
            Acer,
            Fujitsu,
            Zte,
            Huawei,
            Archos,
            Microsoft,
            Ktouch,
            Lava,
            Polaroid,
            Trekstor,
            Hp,
            Hisense,
            Alcatel
        }
    
        /// <summary>
        /// Dictionary that matches the vendor based on a vendor id. Some vendors are not officially registered and use Microsofts id. An example of this is BLU.
        /// </summary>
        private static readonly Dictionary<String, Vendors> VENDOR_IDS = new Dictionary<String, Vendors>()
        {
            { "0421", Vendors.Nokia },
            { "0BB4", Vendors.Htc  },
            { "1004", Vendors.Lg },
            { "413C", Vendors.Dell },
            { "04E8", Vendors.Samsung },
            { "0502", Vendors.Acer },
            { "0BF8", Vendors.Fujitsu },
            { "04C5", Vendors.Fujitsu },
            { "19D2", Vendors.Zte },
            { "12D1", Vendors.Huawei },
            { "0E79", Vendors.Archos },
            { "045E", Vendors.Microsoft },
            { "24E3", Vendors.Ktouch },
            { "0AA3", Vendors.Lava },
            { "0546", Vendors.Polaroid },
            { "0836", Vendors.Trekstor },
            { "1E68", Vendors.Trekstor },
            { "F003", Vendors.Hp },
            { "109B", Vendors.Hisense },
            { "1BBB", Vendors.Alcatel }
        };

        /// <summary>
        /// Check if the vendor exists based on the product name.
        /// </summary>
        /// <param name="name">Name of the product.</param>
        /// <returns>Matching vendor, null if not matching.</returns>
        public static Vendors? Contains(String name)
        {
            foreach (Vendors vendor in Enum.GetValues(typeof(Vendors)))
                if (name.ToLower().Contains(vendor.ToString().ToLower()))
                    return vendor;
            return null;
        }

        /// <summary>
        /// Check if the vendor exists based on the vendor id.
        /// </summary>
        /// <param name="vendorId">Identifier of the vendor.</param>
        /// <returns>Matching vendor, null if not matching.</returns>
        public static Vendors? GetVendor(String vendorId)
        {
            if (vendorId != null)
            {
                Vendors vendor;
                if (VENDOR_IDS.TryGetValue(vendorId, out vendor))
                    return vendor;
            }
            return null;
        }
    }
}
