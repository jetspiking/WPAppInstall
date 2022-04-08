using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPAppInstall.Interfaces;
using WPAppInstall.Resources.Strings;
using WPAppInstall.WindowsPhone.Hardware;

namespace WPAppInstall.Fragments
{
    /// <summary>
    /// This class builds the view / window to view devices. 
    /// </summary>
    public class Devices : IFragment
    {
        private readonly StackPanel rootPanel = new StackPanel();
        private readonly Label startLabel = new Label();

        /// <summary>
        /// Build the view that contains the various (USB) devices.
        /// </summary>
        public Devices()
        {
            foreach (USBScanner.USBDevice device in Misc.Application.Lifecycle.DevicesUSB)
            {
                StackPanel horizontalPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                StackPanel devicePanel = new StackPanel();

                USBConnectionHandler.USBDevice usbDeviceProperty = new USBConnectionHandler.USBDevice(device.PnpDeviceId);

                String vid = $"{AppStrings.HARDWARE_USB_VID}: {usbDeviceProperty.VendorId}";
                String pid = $"{AppStrings.HARDWARE_USB_PID}: {usbDeviceProperty.ProductId}";

                Label deviceName = new Label();
                Label deviceManufacturer = new Label();
                Label deviceDescription = new Label();
                Label deviceService = new Label();
                Label devicePNPClass = new Label();
                Label devicePNPID = new Label();
                Label deviceVID = new Label();
                Label devicePID = new Label();
                Label deviceCaption = new Label();
                Label deviceGUID = new Label();
                Label deviceStatus = new Label();
                Image deviceImage = new Image();

                deviceName.Content = $"Name: {device.Name}";
                deviceManufacturer.Content = $"Manufacturer: {device.Manufacturer}";
                deviceDescription.Content = $"Description: {device.Description}";
                deviceService.Content = $"Service: {device.Service}";
                devicePNPClass.Content = $"PNPClass: {device.PnpClass}";
                devicePNPID.Content = $"PNPID: {device.PnpDeviceId}";
                deviceVID.Content = vid;
                devicePID.Content = pid;
                deviceCaption.Content = $"Caption: {device.Caption}";
                deviceGUID.Content = $"GUID: {device.ClassGuid}";
                deviceStatus.Content = $"Status: {device.Status}";

                deviceName.FontFamily = new System.Windows.Media.FontFamily(Misc.Application.Text.FONT);
                deviceManufacturer.FontFamily = new System.Windows.Media.FontFamily(Misc.Application.Text.FONT);

                deviceName.FontWeight = FontWeights.Bold;

                BitmapFrame image = Misc.Image.GetDeviceImage(device.PnpDeviceId);

                if (image != null)
                {
                    deviceImage.Source = image;
                    deviceImage.HorizontalAlignment = HorizontalAlignment.Left;
                    deviceImage.Width = 300;
                }

                devicePanel.Children.Add(new Separator());
                devicePanel.Children.Add(deviceName);
                devicePanel.Children.Add(deviceManufacturer);
                devicePanel.Children.Add(deviceDescription);
                devicePanel.Children.Add(deviceService);
                devicePanel.Children.Add(devicePNPClass);
                devicePanel.Children.Add(devicePNPID);
                devicePanel.Children.Add(deviceVID);
                devicePanel.Children.Add(devicePID);
                devicePanel.Children.Add(deviceCaption);
                devicePanel.Children.Add(deviceGUID);
                devicePanel.Children.Add(deviceStatus);
                devicePanel.Margin = new Thickness(0, 0, 0, 30);

                horizontalPanel.Children.Add(deviceImage);
                horizontalPanel.Children.Add(devicePanel);

                rootPanel.Children.Add(horizontalPanel);
            }
        }

        /// <summary>
        /// Get the root of this view for displaying purposes.
        /// </summary>
        /// <returns>Root of this view.</returns>
        public StackPanel GetRoot()
        {
            return rootPanel;
        }
    }
}
