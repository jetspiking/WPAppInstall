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
    /// Scan for usb devices using a management-query.
    /// </summary>
    public static class USBScanner
    {
        private static readonly String propertyDeviceId = "DeviceID";
        private static readonly String propertyPnpDeviceId = "PNPDeviceID";
        private static readonly String propertyManufacturer = "Manufacturer";
        private static readonly String propertyService = "Service";
        private static readonly String propertyPnpClass = "PNPClass";
        private static readonly String propertySystemName = "SystemName";
        private static readonly String propertyDescription = "Description";
        private static readonly String propertyName = "Name";
        private static readonly String propertyCaption = "Caption";
        private static readonly String propertyStatus = "Status";
        private static readonly String propertyClassGuid = "ClassGuid";
        private static readonly String usbQuery = $"Select * From Win32_PnPEntity";
        public static readonly USBScanner.FilterProperties FilterPropertiesWindowsPhone = new USBScanner.FilterProperties((uint)USBScanner.FilterProperties.DeviceIdFilters.Usb, (uint)USBScanner.FilterProperties.PNPClassFilters.Wpd);

        /// <summary>
        /// Properties for usb devices.
        /// </summary>
        public class USBDevice
        {
            public String DeviceId;
            public String PnpDeviceId;
            public String Manufacturer;
            public String Service;
            public String PnpClass;
            public String SystemName;
            public String Description;
            public String Name;
            public String Caption;
            public String Status;
            public String ClassGuid;

            public USBDevice()
            {
            }
        }

        /// <summary>
        /// Properties for filtering usb devices.
        /// </summary>
        public class FilterProperties
        {
            [Flags]
            public enum DeviceIdFilters : UInt32
            {
                Any = 0,
                Usb = 1,
            }

            [Flags]
            public enum PNPClassFilters : UInt32
            {
                Any = 0,
                Wpd = 1,
                UsbDevice = 2,
            }

            public readonly UInt32 deviceIdFilters;
            public readonly UInt32 pnpClassFilters;

            /// <summary>
            /// Create the filter properties.
            /// </summary>
            /// <param name="deviceIdFilters">Filter for the device id.</param>
            /// <param name="pnpClassFilters">Filter for the pnp class.</param>
            public FilterProperties(UInt32 deviceIdFilters, UInt32 pnpClassFilters)
            {
                this.deviceIdFilters = deviceIdFilters;
                this.pnpClassFilters = pnpClassFilters;

            }
        }

        /// <summary>
        /// Get a list of the usb devices matching the specified filter.
        /// </summary>
        /// <param name="filterProperties">Filters applicable for getting the list.</param>
        /// <returns>List of usb devices matching the specified filter.</returns>
        public static List<USBDevice> GetUSBDevices(FilterProperties filterProperties)
        {
            List<USBDevice> devices = new List<USBDevice>();

            ManagementObjectCollection collection;

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(usbQuery))
                collection = searcher.Get();

            foreach (ManagementBaseObject device in collection)
            {
                USBDevice usbDevice = new USBDevice();

                String deviceId = (String)device.GetPropertyValue(propertyDeviceId);
                usbDevice.DeviceId = deviceId;

                String pnpDeviceId = (String)device.GetPropertyValue(propertyPnpDeviceId);
                usbDevice.PnpDeviceId = pnpDeviceId;

                String service = (String)device.GetPropertyValue(propertyService);
                usbDevice.Service = service;

                String pnpClass = (String)device.GetPropertyValue(propertyPnpClass);
                usbDevice.PnpClass = pnpClass;

                String manufacturer = (String)device.GetPropertyValue(propertyManufacturer);
                usbDevice.Manufacturer = manufacturer;

                String systemName = (String)device.GetPropertyValue(propertySystemName);
                usbDevice.SystemName = systemName;

                String descriptionId = (String)device.GetPropertyValue(propertyDescription);
                usbDevice.Description = descriptionId;

                String name = (String)device.GetPropertyValue(propertyName);
                usbDevice.Name = name;

                String caption = (String)device.GetPropertyValue(propertyCaption);
                usbDevice.Caption = caption;

                String status = (String)device.GetPropertyValue(propertyStatus);
                usbDevice.Status = status;

                String classGuid = (String)device.GetPropertyValue(propertyClassGuid);
                usbDevice.ClassGuid = classGuid;

                if ((deviceId != null) && (filterProperties.deviceIdFilters & (UInt32)DeviceIdFilters.Usb) == (UInt32)DeviceIdFilters.Usb)
                    if (!deviceId.ToLower().Contains(DeviceIdFilters.Usb.ToString().ToLower())) continue;

                if ((pnpClass != null) && (filterProperties.pnpClassFilters & (UInt32)PNPClassFilters.Wpd) == (UInt32)PNPClassFilters.Wpd)
                    if (!pnpClass.ToLower().Contains(PNPClassFilters.Wpd.ToString().ToLower())) continue;

                if ((pnpClass != null) && (filterProperties.pnpClassFilters & (UInt32)PNPClassFilters.UsbDevice) == (UInt32)PNPClassFilters.UsbDevice)
                    if (!pnpClass.ToLower().Contains(PNPClassFilters.UsbDevice.ToString().ToLower())) continue;
                
                devices.Add(usbDevice);
            }

            return devices;
        }
    }
}
