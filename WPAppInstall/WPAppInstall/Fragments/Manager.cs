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
        private StackPanel stackPanel = new StackPanel();
        private Microsoft.Phone.Tools.Deploy.DeviceInfo selectedDeviceInfo;
        private Microsoft.Phone.Tools.Deploy.DeviceInfo[] devicesInfo;
        private Grid deviceSpecificationsGrid = new Grid();
        private Grid deviceApplicationsGrid = new Grid();
        private Int32 specificationRowIndex = 0;
        private Int32 applicationRowIndex = 0;

        /// <summary>
        /// Build the view or page that is used for managing installed applications.
        /// </summary>
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
            stackPanel.Children.Add(managerReadLabel);

            ComboBox managerDeviceCombobox = new ComboBox
            {
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left,
                IsEditable = true,
                IsReadOnly = true
            };
            devicesInfo = Microsoft.Phone.Tools.Deploy.Utils.GetDevices();
            if (devicesInfo.Length > 0)
            {
                selectedDeviceInfo = devicesInfo[0];
                managerDeviceCombobox.SelectedItem = selectedDeviceInfo;
            }
            foreach (Microsoft.Phone.Tools.Deploy.DeviceInfo device in devicesInfo)
                managerDeviceCombobox.Items.Add(device);
            managerDeviceCombobox.SelectionChanged += ManagerDeviceCombobox_SelectionChanged; ;
            stackPanel.Children.Add(managerDeviceCombobox);

            Button managerReadButton = new Button
            {
                Content = Resources.Strings.AppStrings.MANAGER_READ,
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 200,
                Margin = new Thickness(0, 10, 0, 0)
            };
            managerReadButton.Click += ManagerReadButton_Click;
            stackPanel.Children.Add(managerReadButton);

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
            stackPanel.Children.Add(deviceSpecificationsLabel);

            ColumnDefinition deviceSpecification = new ColumnDefinition();
            ColumnDefinition deviceValue = new ColumnDefinition();
            deviceSpecificationsGrid.ColumnDefinitions.Add(deviceSpecification);
            deviceSpecificationsGrid.ColumnDefinitions.Add(deviceValue);
            stackPanel.Children.Add(deviceSpecificationsGrid);

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
            stackPanel.Children.Add(deviceApplicationsLabel);

            ColumnDefinition appId = new ColumnDefinition();
            ColumnDefinition appLaunchButton = new ColumnDefinition();
            ColumnDefinition appTerminateButton = new ColumnDefinition();
            ColumnDefinition appUninstallButton = new ColumnDefinition();
            deviceApplicationsGrid.ColumnDefinitions.Add(appId);
            deviceApplicationsGrid.ColumnDefinitions.Add(appLaunchButton);
            deviceApplicationsGrid.ColumnDefinitions.Add(appTerminateButton);
            deviceApplicationsGrid.ColumnDefinitions.Add(appUninstallButton);
            stackPanel.Children.Add(deviceApplicationsGrid);
        }

        /// <summary>
        /// Change the selected device when the selection of the combobox changed.
        /// </summary>
        private void ManagerDeviceCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDeviceInfo = (Microsoft.Phone.Tools.Deploy.DeviceInfo)((ComboBox)sender).SelectedItem;
        }

        /// <summary>
        /// Read the device specifications and installed applications.
        /// </summary>
        private void ManagerReadButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDeviceInfo == devicesInfo[0] && Misc.Application.Lifecycle.DevicesUSB.Count == 0)
            {
                BitmapImage warningImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_WARNING, Misc.Image.Extensions.png);
                DialogPopup dialogPopup = new DialogPopup(AppStrings.MANAGER_WARNING, AppStrings.MANAGER_CONNECT_DEVICE, null, DialogPopup.DefaultButtons.Ok, warningImage, false);
                dialogPopup.Show();
                return;
            }

            BitmapImage infoImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_TIP, Misc.Image.Extensions.png);
            DialogPopup infoDialogPopup = new DialogPopup(AppStrings.MANAGER_INFO, AppStrings.MANAGER_READING, null, DialogPopup.DefaultButtons.None, infoImage, false);
            infoDialogPopup.Show();

            deviceSpecificationsGrid.Children.Clear();
            deviceApplicationsGrid.Children.Clear();
            specificationRowIndex = 0;
            applicationRowIndex = 0;

            if (Misc.Application.Lifecycle.DevicesUSB.Count > 1 && selectedDeviceInfo == devicesInfo[0])
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
                        if (device.Name == selectedDeviceInfo.ToString())
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

        /// <summary>
        /// Add a device specification to the view.
        /// </summary>
        /// <param name="specification">Device specification label.</param>
        /// <param name="value">Specification value.</param>
        private void AddSpecificationsGridEntry(String specification, String value)
        {
            RowDefinition row = new RowDefinition();
            deviceSpecificationsGrid.RowDefinitions.Add(row);
            Label specificationLabel = new Label
            {
                Content = specification
            };
            Label valueLabel = new Label
            {
                Content = value
            };
            specificationLabel.SetValue(Grid.ColumnProperty, 0);
            specificationLabel.SetValue(Grid.RowProperty, specificationRowIndex);
            valueLabel.SetValue(Grid.ColumnProperty, 1);
            valueLabel.SetValue(Grid.RowProperty, specificationRowIndex);
            deviceSpecificationsGrid.Children.Add(specificationLabel);
            deviceSpecificationsGrid.Children.Add(valueLabel);
            specificationRowIndex++;
        }

        /// <summary>
        /// Add an application to the grid.
        /// </summary>
        /// <param name="appId">App that should be displayed.</param>
        /// <param name="remoteApplication">Remote application.</param>
        private void AddApplicationsGridEntry(String appId, Microsoft.SmartDevice.Connectivity.Interface.IRemoteApplication remoteApplication)
        {
            RowDefinition row = new RowDefinition();
            deviceApplicationsGrid.RowDefinitions.Add(row);
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
            appIdLabel.SetValue(Grid.RowProperty, applicationRowIndex);
            launchAppButton.SetValue(Grid.ColumnProperty, 1);
            launchAppButton.SetValue(Grid.RowProperty, applicationRowIndex);
            terminateAppButton.SetValue(Grid.ColumnProperty, 2);
            terminateAppButton.SetValue(Grid.RowProperty, applicationRowIndex);
            uninstallAppButton.SetValue(Grid.ColumnProperty, 3);
            uninstallAppButton.SetValue(Grid.RowProperty, applicationRowIndex);
            deviceApplicationsGrid.Children.Add(appIdLabel);
            deviceApplicationsGrid.Children.Add(launchAppButton);
            deviceApplicationsGrid.Children.Add(terminateAppButton);
            deviceApplicationsGrid.Children.Add(uninstallAppButton);
            applicationRowIndex++;
        }

        /// <summary>
        /// Terminate the corresponding application on a button click action.
        /// </summary>
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

        /// <summary>
        /// Uninstall the corresponding application on a button click action.
        /// </summary>
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

        /// <summary>
        /// Launch the corresponding application on a button click action.
        /// </summary>
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

        /// <summary>
        /// Show an error dialog for when an error occurs in the manager.
        /// </summary>
        /// <param name="error">Error to display.</param>
        private void ShowErrorDialog(String error)
        {
            BitmapImage errorImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_ERROR, Misc.Image.Extensions.png);
            DialogPopup dialogPopup = new DialogPopup(AppStrings.MANAGER_ERROR, AppStrings.MANAGER_MICROSOFT_DEPLOY_ERROR+error, null, DialogPopup.DefaultButtons.None, errorImage, false);
            dialogPopup.Show();
        }

        /// <summary>
        /// Get the root of this view for displaying purposes.
        /// </summary>
        /// <returns>Root of this view.</returns>
        public StackPanel GetRoot()
        {
            return stackPanel;
        }
    }
}
