using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPAppInstall.Fragments;
using WPAppInstall.WindowsPhone.Hardware;
using WPAppInstall.Misc;
using WPAppInstall.Resources.Strings;
using System.Threading;
using System.Management;
using WPAppInstall.Interfaces;

namespace WPAppInstall
{
    /// <summary>
    /// Main application window.
    /// </summary>

    public partial class MainWindow : Window, IThemeUpdatable, IUSBSubsriber, INavigator, IResizable
    {

        private USBConnectionHandler _windowsPhoneHandlerUSB;
        private DialogPopup _dialogUSB;

        public MainWindow()
        {

            InitializeComponent();
            Title = AppStrings.APP_NAME;
            AppDescription.Content = AppStrings.APP_DESCRIPTION;
            AppDescription.Foreground = new SolidColorBrush(Misc.Application.Lifecycle.ApplicationColor);
            AppDescription.FontWeight = FontWeights.Bold;

            StartButton.BorderThickness = new Thickness(0, 0, 0, Misc.Application.Margin.SELECTED_PAGE_BAR_HEIGHT);
            DevicesButton.BorderThickness = new Thickness(0, 0, 0, Misc.Application.Margin.SELECTED_PAGE_BAR_HEIGHT);
            AppsButton.BorderThickness = new Thickness(0, 0, 0, Misc.Application.Margin.SELECTED_PAGE_BAR_HEIGHT);
            DeployButton.BorderThickness = new Thickness(0, 0, 0, Misc.Application.Margin.SELECTED_PAGE_BAR_HEIGHT);
            SettingsButton.BorderThickness = new Thickness(0, 0, 0, Misc.Application.Margin.SELECTED_PAGE_BAR_HEIGHT);

            PreviousMenuButton.Click += PreviousMenuButton_Click;
            DevicesMenuButton.Click += DevicesButton_Click;
            AppsMenuButton.Click += AppsButton_Click;
            DeployMenuButton.Click += DeployButton_Click;
            SettingsMenuButton.Click += SettingsButton_Click;
            StartMenuButton.Click += StartButton_Click;
            ExitMenuButton.Click += ExitMenuButton_Click;
            ManagerMenuButton.Click += ManagerMenuButton_Click;

            StartButton.Click += StartButton_Click;
            DevicesButton.Click += DevicesButton_Click;
            AppsButton.Click += AppsButton_Click;
            DeployButton.Click += DeployButton_Click;
            SettingsButton.Click += SettingsButton_Click;

            WPLogoImage.Source = Misc.Image.GetIcon(Misc.Application.Lifecycle.ApplicationTheme);
            WPSeperatorImage.Source = Misc.Image.GetSeperator(Misc.Application.Lifecycle.ApplicationTheme);

            PageContent.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            PageContent.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            this.UseLayoutRounding = true;
            
            SelectPage(Misc.Application.Lifecycle.ApplicationPage);
            _windowsPhoneHandlerUSB = new USBConnectionHandler(this);

        }

        private void ManagerMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (Misc.Application.Lifecycle.ApplicationPage != AppPages.Manager)
            {
                DeselectPage(Misc.Application.Lifecycle.ApplicationPage);
                SelectPage(AppPages.Manager);
            }
        }

        private void AppsButton_Click(object sender, RoutedEventArgs e)
        {
            if (Misc.Application.Lifecycle.ApplicationPage != AppPages.Apps)
            {
                DeselectPage(Misc.Application.Lifecycle.ApplicationPage);
                SelectPage(AppPages.Apps);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (Misc.Application.Lifecycle.ApplicationPage != AppPages.Start)
            {
                DeselectPage(Misc.Application.Lifecycle.ApplicationPage);
                SelectPage(AppPages.Start);
            }
        }

        private void PreviousMenuButton_Click(object sender, RoutedEventArgs e)
        {
            AppPages? appPage = Misc.Application.Lifecycle.PopPage();
            PreviousMenuButton.IsEnabled = appPage != null;

            if (appPage != null)
            {
                PreviousMenuButton.IsEnabled = Misc.Application.Lifecycle.HistoryContainsMorePages();
                DeselectPage(Misc.Application.Lifecycle.ApplicationPage, true);
                SelectPage((AppPages)appPage);
            }
            else
                PreviousMenuButton.IsEnabled = false;
        }

        private void ExitMenuButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

            if (Misc.Application.Lifecycle.ApplicationPage != AppPages.Settings)
            {
                DeselectPage(Misc.Application.Lifecycle.ApplicationPage);
                SelectPage(AppPages.Settings);
            }
        }

        private void DeployButton_Click(object sender, RoutedEventArgs e)
        {
            if (Misc.Application.Lifecycle.ApplicationPage != AppPages.Deploy)
            {
                DeselectPage(Misc.Application.Lifecycle.ApplicationPage);
                SelectPage(AppPages.Deploy);
            }
        }

        private void DevicesButton_Click(object sender, RoutedEventArgs e)
        {
            if (Misc.Application.Lifecycle.ApplicationPage != AppPages.Devices)
            {
                DeselectPage(Misc.Application.Lifecycle.ApplicationPage);
                SelectPage(AppPages.Devices);
            }
        }

        private void SelectPage(AppPages page)
        {
            switch (page)
            {
                case AppPages.Start:
                    PageContent.Content = new Fragments.Start(this).GetRoot();
                    StartButton.BorderBrush = new SolidColorBrush(Misc.Application.Lifecycle.ApplicationColor);
                    break;
                case AppPages.Apps:
                    PageContent.Content = null;
                    AppsButton.BorderBrush = new SolidColorBrush(Misc.Application.Lifecycle.ApplicationColor);
                    PageContent.Content = new Fragments.Apps().GetRoot();
                    break;
                case AppPages.Devices:
                    PageContent.Content = null;
                    DevicesButton.BorderBrush = new SolidColorBrush(Misc.Application.Lifecycle.ApplicationColor);
                    PageContent.Content = new Fragments.Devices().GetRoot();
                    break;
                case AppPages.Manager:
                    PageContent.Content = null;
                    PageContent.Content = new Fragments.Manager().GetRoot();
                    break;
                case AppPages.Deploy:
                    PageContent.Content = null;
                    DeployButton.BorderBrush = new SolidColorBrush(Misc.Application.Lifecycle.ApplicationColor);
                    PageContent.Content = new Fragments.Deploy().GetRoot();
                    break;
                case AppPages.Settings:
                    PageContent.Content = null;
                    SettingsButton.BorderBrush = new SolidColorBrush(Misc.Application.Lifecycle.ApplicationColor);
                    PageContent.Content = new Fragments.Settings(this,this).GetRoot();
                    break;
            }
            Misc.Application.Lifecycle.ApplicationPage = page;
        }

        private void DeselectPage(AppPages page, bool isPreviousPage = false)
        {
            switch (page)
            {
                case AppPages.Start:
                    StartButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    break;
                case AppPages.Apps:
                    AppsButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    break;
                case AppPages.Devices:
                    DevicesButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    break;
                case AppPages.Deploy:
                    DeployButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    break;
                case AppPages.Settings:
                    SettingsButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    break;
            }

            if (!isPreviousPage)
            {
                Misc.Application.Lifecycle.AddPage(page);
                PreviousMenuButton.IsEnabled = true;
            }
        }

        public void UpdateAppTheme()
        {
            AppDescription.Foreground = new SolidColorBrush(Misc.Application.Lifecycle.ApplicationColor);
            WPLogoImage.Source = Misc.Image.GetIcon(Misc.Application.Lifecycle.ApplicationTheme);
            WPSeperatorImage.Source = Misc.Image.GetSeperator(Misc.Application.Lifecycle.ApplicationTheme);
        }

        private void USBPopup(USBConnectionHandler.USBActions action, String name)
        {
            Thread.Sleep(500); // Time needed to add more potential devices after this request (see USBConnectionHandler). 

            USBConnectionHandler.USBDevice usbProperty = new USBConnectionHandler.USBDevice(name);
            VendorIds.Vendors? vendor = VendorIds.GetVendor(usbProperty.vendorId);

            if (vendor == null) return;

            String vid = $"{AppStrings.HARDWARE_USB_VID}: {usbProperty.vendorId} ({((VendorIds.Vendors)vendor).ToString()})";
            String pid = $"{AppStrings.HARDWARE_USB_PID}: {usbProperty.productId}";
            String guid = $"{AppStrings.HARDWARE_USB_GUID}: {usbProperty.guid}";

            String pidAddition = ProductIds.GetPIDAddition(usbProperty.productId);
            if (pidAddition != null)
                pid += $" ({ProductIds.GetPIDAddition(usbProperty.productId)})";
            else if (usbProperty.productId != AppStrings.APP_WINDOWS_PHONE_7_PID)
                pid += $" ({AppStrings.APP_WINDOWS_PHONE_8_10_DEVICE})";

            String content = String.Empty;

            USBScanner.USBDevice usbDevice = null;

            if (action == USBConnectionHandler.USBActions.Connect) 
                Misc.Application.Lifecycle.DevicesUSB = USBScanner.GetUSBDevices(USBScanner.FILTER_PROPERTIES_WINDOWS_PHONE);

            Misc.Application.Lifecycle.DevicesUSB.ForEach(device =>
            {
                USBConnectionHandler.USBDevice usbDeviceProperty = new USBConnectionHandler.USBDevice(device.pnpDeviceId);

                if (usbDeviceProperty.productId == usbProperty.productId && usbDeviceProperty.vendorId == usbProperty.vendorId) //  && usbDeviceProperty.vendorId == usbProperty.vendorId
                {
                    usbDevice = device;

                    VendorIds.Vendors? vendorContains = VendorIds.Contains(device.name);
                    if (vendorContains != null)
                        vendor = vendorContains;

                    if (ProductIds.Contains(device.name))
                        return;
                }
            });

            BitmapSource deviceImage=null;

            if (usbDevice != null)
            {
                content += $"\n{AppStrings.HARDWARE_USB_CAPTION}: {usbDevice.caption.ToUpper()}\n\n{AppStrings.HARDWARE_USB_MANUFACTURER}: {usbDevice.manufacturer}\n{AppStrings.HARDWARE_USB_DESCRIPTION}: {usbDevice.description}\n{AppStrings.HARDWARE_USB_STATUS}: {usbDevice.status}\n\n";
                deviceImage = Misc.Image.GetDeviceImage(usbDevice.pnpDeviceId);
            }


            content += $"{vid}\n{pid}\n{guid}";

            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (_dialogUSB != null)
                    _dialogUSB.Close();

                if (deviceImage == null)
                    deviceImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_CELLPHONE, Misc.Image.Extensions.png);

                _dialogUSB = new DialogPopup(action == USBConnectionHandler.USBActions.Connect ? AppStrings.HARDWARE_CONNECTED_USB_DEVICE : AppStrings.HARDWARE_DISCONNECTED_USB_DEVICE, content, Misc.Image.GetBrandLogo((VendorIds.Vendors)vendor), DialogPopup.DefaultButtons.Ok, deviceImage);
                _dialogUSB.Show();
            }));
        }

        public void NotifyConnected(String name)
        {
            USBPopup(USBConnectionHandler.USBActions.Connect, name);
        }

        public void NotifyDisconnected(String name)
        {
            USBPopup(USBConnectionHandler.USBActions.Disconnect, name);
        }

        public void Navigate(AppPages appPage)
        {
            DeselectPage(Misc.Application.Lifecycle.ApplicationPage);
            SelectPage(appPage);
        }

        public void SetResizable(bool enabled)
        {
            this.ResizeMode = enabled ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize;
        }
    }
}
