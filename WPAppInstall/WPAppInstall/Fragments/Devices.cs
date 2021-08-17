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
        private readonly StackPanel _rootPanel = new StackPanel();
        private readonly Label _startLabel = new Label();

        public Devices()
        {
            foreach (USBScanner.USBDevice device in Misc.Application.Lifecycle.DevicesUSB)
            {
                StackPanel horizontalPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                StackPanel devicePanel = new StackPanel();

                USBConnectionHandler.USBDevice usbDeviceProperty = new USBConnectionHandler.USBDevice(device.pnpDeviceId);

                String vid = $"{AppStrings.HARDWARE_USB_VID}: {usbDeviceProperty.vendorId}";
                String pid = $"{AppStrings.HARDWARE_USB_PID}: {usbDeviceProperty.productId}";

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
                Button deviceDetails = new Button();
                Image deviceImage = new Image();

                deviceName.Content = $"Name: {device.name}";
                deviceManufacturer.Content = $"Manufacturer: {device.manufacturer}";
                deviceDescription.Content = $"Description: {device.description}";
                deviceService.Content = $"Service: {device.service}";
                devicePNPClass.Content = $"PNPClass: {device.pnpClass}";
                devicePNPID.Content = $"PNPID: {device.pnpDeviceId}";
                deviceVID.Content = vid;
                devicePID.Content = pid;
                deviceCaption.Content = $"Caption: {device.caption}";
                deviceGUID.Content = $"GUID: {device.classGuid}";
                deviceStatus.Content = $"Status: {device.status}";

                deviceName.FontFamily = new System.Windows.Media.FontFamily(Misc.Application.Text.FONT);
                deviceManufacturer.FontFamily = new System.Windows.Media.FontFamily(Misc.Application.Text.FONT);

                deviceName.FontWeight = FontWeights.Bold;

                CheckBox deployToDevice = new CheckBox
                {
                    Content = $"Deploy to {device.name}",
                    Margin = new Thickness(5, 0, 0, 0)
                };

                BitmapFrame image = Misc.Image.GetDeviceImage(device.pnpDeviceId);

                if (image != null)
                {
                    deviceImage.Source = image;
                    deviceImage.HorizontalAlignment = HorizontalAlignment.Left;
                    deviceImage.Width = 300;
                }

                devicePanel.Children.Add(new Separator());
                devicePanel.Children.Add(deviceName);
                //devicePanel.Children.Add(deployToDevice);
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

                _rootPanel.Children.Add(horizontalPanel);
            }
        }

        public StackPanel GetRoot()
        {
            return _rootPanel;
        }

        public void Update(List<USBScanner.USBDevice> devices)
        {

        }
    }
}
