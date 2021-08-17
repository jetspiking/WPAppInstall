using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using static WPAppInstall.WindowsPhone.Hardware.USBScanner.FilterProperties;

namespace WPAppInstall.WindowsPhone.Hardware
{
    /// <summary>
    /// Scan for USB-devices using a management-query.
    /// </summary>

    public static class USBScanner
    {
        private static readonly String PROPERTY_DEVICE_ID = "DeviceID";
        private static readonly String PROPERTY_PNP_DEVICE_ID = "PNPDeviceID";
        private static readonly String PROPERTY_MANUFACTURER = "Manufacturer";
        private static readonly String PROPERTY_SERVICE = "Service";
        private static readonly String PROPERTY_PNP_CLASS = "PNPClass";
        private static readonly String PROPERTY_SYSTEM_NAME = "SystemName";
        private static readonly String PROPERTY_DESCRIPTION = "Description";
        private static readonly String PROPERTY_NAME = "Name";
        private static readonly String PROPERTY_CAPTION = "Caption";
        private static readonly String PROPERTY_STATUS = "Status";
        private static readonly String PROPERTY_CLASS_GUID = "ClassGuid";
        private static readonly String USB_QUERY = $"Select * From Win32_PnPEntity"; // Win32_PnPEntity  // Select * From Win32_PnPEntity Where {PROPERTY_DEVICE_ID} Like \"USB%\"
        public static readonly USBScanner.FilterProperties FILTER_PROPERTIES_WINDOWS_PHONE = new USBScanner.FilterProperties((uint)USBScanner.FilterProperties.DeviceIdFilters.USB, (uint)USBScanner.FilterProperties.PNPClassFilters.WPD);

        public class USBDevice
        {
            public String deviceId;
            public String pnpDeviceId;
            public String manufacturer;
            public String service;
            public String pnpClass;
            public String systemName;
            public String description;
            public String name;
            public String caption;
            public String status;
            public String classGuid;

            public USBDevice()
            {
            }
        }

        public class FilterProperties
        {
            [Flags]
            public enum DeviceIdFilters : uint
            {
                ANY = 0,
                USB = 1,
            }

            [Flags]
            public enum PNPClassFilters : uint
            {
                ANY = 0,
                WPD = 1,
                USBDevice = 2,
            }

            public readonly uint deviceIdFilters;
            public readonly uint pnpClassFilters;

            public FilterProperties(uint deviceIdFilters, uint pnpClassFilters)
            {
                this.deviceIdFilters = deviceIdFilters;
                this.pnpClassFilters = pnpClassFilters;

            }
        }

        public static List<USBDevice> GetUSBDevices(FilterProperties filterProperties)
        {
            List<USBDevice> devices = new List<USBDevice>();

            ManagementObjectCollection collection;

            using (var searcher = new ManagementObjectSearcher(USB_QUERY))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                USBDevice usbDevice = new USBDevice();

                String deviceId = (String)device.GetPropertyValue(PROPERTY_DEVICE_ID);
                usbDevice.deviceId = deviceId;

                String pnpDeviceId = (String)device.GetPropertyValue(PROPERTY_PNP_DEVICE_ID);
                usbDevice.pnpDeviceId = pnpDeviceId;

                String service = (String)device.GetPropertyValue(PROPERTY_SERVICE);
                usbDevice.service = service;

                String pnpClass = (String)device.GetPropertyValue(PROPERTY_PNP_CLASS);
                usbDevice.pnpClass = pnpClass;

                String manufacturer = (String)device.GetPropertyValue(PROPERTY_MANUFACTURER);
                usbDevice.manufacturer = manufacturer;

                String systemName = (String)device.GetPropertyValue(PROPERTY_SYSTEM_NAME);
                usbDevice.systemName = systemName;

                String descriptionId = (String)device.GetPropertyValue(PROPERTY_DESCRIPTION);
                usbDevice.description = descriptionId;

                String name = (String)device.GetPropertyValue(PROPERTY_NAME);
                usbDevice.name = name;

                String caption = (String)device.GetPropertyValue(PROPERTY_CAPTION);
                usbDevice.caption = caption;

                String status = (String)device.GetPropertyValue(PROPERTY_STATUS);
                usbDevice.status = status;

                String classGuid = (String)device.GetPropertyValue(PROPERTY_CLASS_GUID);
                usbDevice.classGuid = classGuid;

                if ((deviceId != null) && (filterProperties.deviceIdFilters & (uint)DeviceIdFilters.USB) == (uint)DeviceIdFilters.USB)
                {
                    if (!deviceId.ToLower().Contains(DeviceIdFilters.USB.ToString().ToLower())) continue;
                }

                if ((pnpClass != null) && (filterProperties.pnpClassFilters & (uint)PNPClassFilters.WPD) == (uint)PNPClassFilters.WPD)
                {
                    if (!pnpClass.ToLower().Contains(PNPClassFilters.WPD.ToString().ToLower())) continue;

                }

                if ((pnpClass != null) && (filterProperties.pnpClassFilters & (uint)PNPClassFilters.USBDevice) == (uint)PNPClassFilters.USBDevice)
                {
                    if (!pnpClass.ToLower().Contains(PNPClassFilters.USBDevice.ToString().ToLower())) continue;
                }
                devices.Add(usbDevice);
            }

            return devices;
        }
    }
}
