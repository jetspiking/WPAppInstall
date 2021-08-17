using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPAppInstall.Interfaces;
using WPAppInstall.Resources.Strings;

namespace WPAppInstall.Fragments
{
    /// <summary>
    /// This class builds the view / window to read phone data (manager). 
    /// </summary>

    public class Manager : IFragment
    {
        private StackPanel _stackPanel = new StackPanel();
        private Microsoft.Phone.Tools.Deploy.DeviceInfo _selectedDeviceInfo;
        private Microsoft.Phone.Tools.Deploy.DeviceInfo[] _devicesInfo;
        private Grid _deviceSpecificationsGrid = new Grid();
        private Grid _deviceApplicationsGrid = new Grid();
        private int _specificationRowIndex = 0;
        private int _applicationRowIndex = 0;

        public Manager()
        {
            // Read section
            TextBlock managerReadTextblock = new TextBlock
            {
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Text = Resources.Strings.AppStrings.MANAGER_DEVICE_INFORMATION
            };

            Label managerReadLabel = new Label
            {
                Content = managerReadTextblock
            };
            _stackPanel.Children.Add(managerReadLabel);

            ComboBox managerDeviceCombobox = new ComboBox
            {
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left,
                IsEditable = true,
                IsReadOnly = true
            };
            _devicesInfo = Microsoft.Phone.Tools.Deploy.Utils.GetDevices();
            if (_devicesInfo.Length > 0)
            {
                _selectedDeviceInfo = _devicesInfo[0];
                managerDeviceCombobox.SelectedItem = _selectedDeviceInfo;
            }
            foreach (Microsoft.Phone.Tools.Deploy.DeviceInfo device in _devicesInfo)
                managerDeviceCombobox.Items.Add(device);
            managerDeviceCombobox.SelectionChanged += ManagerDeviceCombobox_SelectionChanged; ;
            _stackPanel.Children.Add(managerDeviceCombobox);

            Button managerReadButton = new Button
            {
                Content = Resources.Strings.AppStrings.MANAGER_READ,
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0)
            };
            managerReadButton.Click += ManagerReadButton_Click;
            _stackPanel.Children.Add(managerReadButton);

            // Device Specs Grid 
            TextBlock deviceSpecificationsTextblock = new TextBlock
            {
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Text = Resources.Strings.AppStrings.MANAGER_DEVICE_SPECIFICATIONS
            };

            Label deviceSpecificationsLabel = new Label
            {
                Content = deviceSpecificationsTextblock
            };
            _stackPanel.Children.Add(deviceSpecificationsLabel);

            ColumnDefinition deviceSpecification = new ColumnDefinition();
            ColumnDefinition deviceValue = new ColumnDefinition();
            _deviceSpecificationsGrid.ColumnDefinitions.Add(deviceSpecification);
            _deviceSpecificationsGrid.ColumnDefinitions.Add(deviceValue);
            _stackPanel.Children.Add(_deviceSpecificationsGrid);

            // Device Apps Grid
            TextBlock deviceApplicationsTextblock = new TextBlock
            {
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Text = Resources.Strings.AppStrings.MANAGER_INSTALLED_APPS
            };

            Label deviceApplicationsLabel = new Label
            {
                Content = deviceApplicationsTextblock
            };
            _stackPanel.Children.Add(deviceApplicationsLabel);

            ColumnDefinition appId = new ColumnDefinition();
            ColumnDefinition appLaunchButton = new ColumnDefinition();
            ColumnDefinition appTerminateButton = new ColumnDefinition();
            ColumnDefinition appUninstallButton = new ColumnDefinition();
            _deviceApplicationsGrid.ColumnDefinitions.Add(appId);
            _deviceApplicationsGrid.ColumnDefinitions.Add(appLaunchButton);
            _deviceApplicationsGrid.ColumnDefinitions.Add(appTerminateButton);
            _deviceApplicationsGrid.ColumnDefinitions.Add(appUninstallButton);
            _stackPanel.Children.Add(_deviceApplicationsGrid);
        }

        private void ManagerDeviceCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedDeviceInfo = (Microsoft.Phone.Tools.Deploy.DeviceInfo)((ComboBox)sender).SelectedItem;
        }

        private void ManagerReadButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDeviceInfo == _devicesInfo[0] && Misc.Application.Lifecycle.DevicesUSB.Count == 0)
            {
                BitmapImage warningImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_WARNING, Misc.Image.Extensions.png);
                DialogPopup dialogPopup = new DialogPopup(AppStrings.MANAGER_WARNING, AppStrings.MANAGER_CONNECT_DEVICE, null, DialogPopup.DefaultButtons.Ok, warningImage, false);
                dialogPopup.Show();
                return;
            }

            BitmapImage infoImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_TIP, Misc.Image.Extensions.png);
            DialogPopup infoDialogPopup = new DialogPopup(AppStrings.MANAGER_INFO, AppStrings.MANAGER_READING, null, DialogPopup.DefaultButtons.None, infoImage, false);
            infoDialogPopup.Show();

            _deviceSpecificationsGrid.Children.Clear();
            _deviceApplicationsGrid.Children.Clear();
            _specificationRowIndex = 0;
            _applicationRowIndex = 0;

            if (Misc.Application.Lifecycle.DevicesUSB.Count > 1 && _selectedDeviceInfo == _devicesInfo[0])
            {
                BitmapImage warningImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_WARNING, Misc.Image.Extensions.png);
                DialogPopup dialogPopup = new DialogPopup(AppStrings.MANAGER_WARNING, AppStrings.MANAGER_DISCONNECT_DEVICES, null, DialogPopup.DefaultButtons.None, warningImage, false);
                dialogPopup.Show();
                return;
            }

            new Thread(() =>
            {
                try
                {

                    System.Collections.ObjectModel.Collection<Microsoft.SmartDevice.MultiTargeting.Connectivity.ConnectableDevice> devices = Misc.Application.Lifecycle.Hardware81.ScanDevices();
                    Microsoft.SmartDevice.MultiTargeting.Connectivity.ConnectableDevice connectableDevice = null;

                    foreach (Microsoft.SmartDevice.MultiTargeting.Connectivity.ConnectableDevice device in devices)
                        if (device.Name == _selectedDeviceInfo.ToString())
                        {
                            connectableDevice = device;
                            break;
                        }

                    if (connectableDevice == null) return;
                    Microsoft.SmartDevice.Connectivity.Interface.IDevice deviceHandle = Misc.Application.Lifecycle.Hardware81.ConnectToDevice(connectableDevice);

                    System.Collections.ObjectModel.Collection<Microsoft.SmartDevice.Connectivity.Interface.IRemoteApplication> apps = Misc.Application.Lifecycle.Hardware81.GetRemoteApplications(deviceHandle);

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Microsoft.SmartDevice.Connectivity.Interface.ISystemInfo systemInfo = deviceHandle.GetSystemInfo();

                        AddSpecificationsGridEntry("Name", deviceHandle.Name);
                        AddSpecificationsGridEntry("Instruction Set", systemInfo.InstructionSet);
                        AddSpecificationsGridEntry("Architecture", systemInfo.ProcessorArchitecture);
                        AddSpecificationsGridEntry("Core Count", systemInfo.NumberOfProcessors.ToString());
                        AddSpecificationsGridEntry("OS Build Number", systemInfo.OSBuildNo.ToString());
                        AddSpecificationsGridEntry("OS Version Major", systemInfo.OSMajor.ToString());
                        AddSpecificationsGridEntry("OS Version Minor", systemInfo.OSMinor.ToString());
                        AddSpecificationsGridEntry("Total Physical", systemInfo.TotalPhys.ToString());
                        AddSpecificationsGridEntry("Available Physical", systemInfo.AvailPhys.ToString());
                        AddSpecificationsGridEntry("Total Virtual", systemInfo.TotalVirtual.ToString());
                        AddSpecificationsGridEntry("Available Virtual", systemInfo.AvailVirtual.ToString());

                        foreach (Microsoft.SmartDevice.Connectivity.Interface.IRemoteApplication remoteApplication in apps)
                            AddApplicationsGridEntry(remoteApplication.ProductID.ToString(), remoteApplication);

                        infoDialogPopup.Close();

                    }));

                    deviceHandle.Disconnect();

                } catch(Exception exception)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ShowErrorDialog(exception.Message);
                    }));
                }

            }).Start();


        }

        private void AddSpecificationsGridEntry(String specification, String value)
        {

            RowDefinition row = new RowDefinition();
            _deviceSpecificationsGrid.RowDefinitions.Add(row);

            Label specificationLabel = new Label
            {
                Content = specification
            };

            Label valueLabel = new Label
            {
                Content = value
            };

            specificationLabel.SetValue(Grid.ColumnProperty, 0);
            specificationLabel.SetValue(Grid.RowProperty, _specificationRowIndex);

            valueLabel.SetValue(Grid.ColumnProperty, 1);
            valueLabel.SetValue(Grid.RowProperty, _specificationRowIndex);

            _deviceSpecificationsGrid.Children.Add(specificationLabel);
            _deviceSpecificationsGrid.Children.Add(valueLabel);

            _specificationRowIndex++;
        }

        private void AddApplicationsGridEntry(String appId, Microsoft.SmartDevice.Connectivity.Interface.IRemoteApplication remoteApplication)
        {

            RowDefinition row = new RowDefinition();
            _deviceApplicationsGrid.RowDefinitions.Add(row);

            Label appIdLabel = new Label
            {
                Content = appId
            };

            Button launchAppButton = new Button
            {
                Content = "Launch",
                Tag = remoteApplication
            };
            launchAppButton.Click += LaunchAppButton_Click;

            Button terminateAppButton = new Button
            {
                Content = "Terminate",
                Tag = remoteApplication
            };
            terminateAppButton.Click += TerminateAppButton_Click;

            Button uninstallAppButton = new Button
            {
                Content = "Uninstall",
                Tag = remoteApplication
            };
            uninstallAppButton.Click += UninstallAppButton_Click;

            appIdLabel.SetValue(Grid.ColumnProperty, 0);
            appIdLabel.SetValue(Grid.RowProperty, _applicationRowIndex);

            launchAppButton.SetValue(Grid.ColumnProperty, 1);
            launchAppButton.SetValue(Grid.RowProperty, _applicationRowIndex);

            terminateAppButton.SetValue(Grid.ColumnProperty, 2);
            terminateAppButton.SetValue(Grid.RowProperty, _applicationRowIndex);

            uninstallAppButton.SetValue(Grid.ColumnProperty, 3);
            uninstallAppButton.SetValue(Grid.RowProperty, _applicationRowIndex);

            _deviceApplicationsGrid.Children.Add(appIdLabel);
            _deviceApplicationsGrid.Children.Add(launchAppButton);
            _deviceApplicationsGrid.Children.Add(terminateAppButton);
            _deviceApplicationsGrid.Children.Add(uninstallAppButton);

            _applicationRowIndex++;
        }

        private void TerminateAppButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                ((Microsoft.SmartDevice.Connectivity.Interface.IRemoteApplication)button.Tag).TerminateRunningInstances();
            }
            catch (Exception exception)
            {
                ShowErrorDialog(exception.Message);
            }
        }

        private void UninstallAppButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                ((Microsoft.SmartDevice.Connectivity.Interface.IRemoteApplication)button.Tag).Uninstall();
            }
            catch (Exception exception)
            {
                ShowErrorDialog(exception.Message);
            }
        }

        private void LaunchAppButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                ((Microsoft.SmartDevice.Connectivity.Interface.IRemoteApplication)button.Tag).Launch();
            }
            catch (Exception exception)
            {
                ShowErrorDialog(exception.Message);
            }
        }

        private void ShowInfoDialog(String info)
        {

        }

        private void ShowErrorDialog(String error)
        {
            BitmapImage errorImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_ERROR, Misc.Image.Extensions.png);
            DialogPopup dialogPopup = new DialogPopup(AppStrings.MANAGER_ERROR, AppStrings.MANAGER_MICROSOFT_DEPLOY_ERROR+error, null, DialogPopup.DefaultButtons.None, errorImage, false);
            dialogPopup.Show();
        }

        public StackPanel GetRoot()
        {
            return _stackPanel;
        }
    }
}
