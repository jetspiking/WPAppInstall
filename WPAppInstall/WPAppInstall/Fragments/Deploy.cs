using Microsoft.Win32;
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
using WPAppInstall.WindowsPhone.Common;
using WPAppInstall.WindowsPhone.Hardware;
using WPAppInstall.WindowsPhone.Hardware8._1;
using static WPAppInstall.WindowsPhone.Hardware8._1.Hardware81;

namespace WPAppInstall.Fragments
{
    /// <summary>
    /// This class builds the view / window to deploy apps. 
    /// </summary>
    public class Deploy : IFragment
    {
        private readonly StackPanel rootPanel = new StackPanel();
        private const String deployerLatest = "Latest (Windows Phone 8.1)";
        private Deployer selectedDeployer = Deployer.Latest;
        private Microsoft.Phone.Tools.Deploy.DeploymentOptions selectedDeploymentOption = Microsoft.Phone.Tools.Deploy.DeploymentOptions.None;
        private String xapPath = String.Empty;
        private Microsoft.Phone.Tools.Deploy.DeviceInfo selectedDeviceInfo;
        private Microsoft.Phone.Tools.Deploy.DeviceInfo[] devicesInfo;
        private DialogPopup deployDialog;

        /// <summary>
        /// Build the view or page that is used for deploying applications.
        /// </summary>
        public Deploy()
        {
            // Main panel
            rootPanel.Orientation = Orientation.Vertical;

            // Deploy version picker 
            StackPanel deployPicker = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Background = new SolidColorBrush(Colors.Transparent)
            };
            rootPanel.Children.Add(deployPicker);

            // Deployer Selector Section
            TextBlock deployerSelectorTextblock = new TextBlock
            {
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Text = Resources.Strings.AppStrings.DEPLOY_DEPLOYER_VERSION
            };

            Label deployerSelectorLabel = new Label
            {
                Content = deployerSelectorTextblock
            };
            deployPicker.Children.Add(deployerSelectorLabel);

            ComboBox deployerSelectorCombobox = new ComboBox
            {
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left,
                IsEditable = true,
                IsReadOnly = true,
                SelectedItem = GetDeployerString(selectedDeployer)
            };
            deployerSelectorCombobox.Items.Add(GetDeployerString(Deployer.Latest));
            deployerSelectorCombobox.SelectionChanged += DeployerSelectorCombobox_SelectionChanged;
            deployPicker.Children.Add(deployerSelectorCombobox);

            // Deployment Option Section
            TextBlock deploymentOptionTextblock = new TextBlock
            {
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Text = Resources.Strings.AppStrings.DEPLOYMENT_OPTION
            };

            Label deploymentOptionLabel = new Label
            {
                Content = deploymentOptionTextblock
            };
            deployPicker.Children.Add(deploymentOptionLabel);

            ComboBox deploymentOptionCombobox = new ComboBox
            {
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left,
                IsEditable = true,
                IsReadOnly = true,
                SelectedItem = GetDeploymentOptionString(selectedDeploymentOption)
            };
            foreach (Microsoft.Phone.Tools.Deploy.DeploymentOptions deploymentOption in (Microsoft.Phone.Tools.Deploy.DeploymentOptions[])Enum.GetValues(typeof(Microsoft.Phone.Tools.Deploy.DeploymentOptions)))
                deploymentOptionCombobox.Items.Add(GetDeploymentOptionString(deploymentOption));
            deploymentOptionCombobox.SelectionChanged += DeploymentOptionCombobox_SelectionChanged;
            deployPicker.Children.Add(deploymentOptionCombobox);

            // Device Deploy Selector
            TextBlock selectedDevicesTextblock = new TextBlock
            {
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Text = "Selected Device"
            };

            Label selectedDevicesLabel = new Label
            {
                Content = selectedDevicesTextblock
            };
            deployPicker.Children.Add(selectedDevicesLabel);

            ComboBox selectedDevicesCombobox = new ComboBox
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
                selectedDevicesCombobox.SelectedItem = selectedDeviceInfo;
            }
            foreach (Microsoft.Phone.Tools.Deploy.DeviceInfo device in devicesInfo)
                selectedDevicesCombobox.Items.Add(device);
            selectedDevicesCombobox.SelectionChanged += SelectedDevicesCombobox_SelectionChanged;
            deployPicker.Children.Add(selectedDevicesCombobox);

            // Browse Apps Section
            TextBlock deploymentAppsTextblock = new TextBlock
            {
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Text = Resources.Strings.AppStrings.DEPLOYMENT_APPS
            };

            Label deploymentAppsLabel = new Label
            {
                Content = deploymentAppsTextblock
            };
            deployPicker.Children.Add(deploymentAppsLabel);

            Label deploymentAppsCountLabel = new Label
            {
                Content = Misc.Application.Lifecycle.Paths.Length.ToString()
            };
            deployPicker.Children.Add(deploymentAppsCountLabel);

            // Deploy Apps Section
            TextBlock deploymentDevicesTextblock = new TextBlock
            {
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Text = Resources.Strings.AppStrings.DEPLOYMENT_DEPLOY
            };

            Label deploymentDevicesLabel = new Label
            {
                Content = deploymentDevicesTextblock
            };
            deployPicker.Children.Add(deploymentDevicesLabel);

            CheckBox deploymentDevicesLaunchCheckbox = new CheckBox
            {
                Content = Resources.Strings.AppStrings.DEPLOYMENT_LAUNCH_AFTER_INSTALLATION,
                Margin = new Thickness(0, 0, 0, 5)
            };
            deploymentDevicesLaunchCheckbox.Checked += DeploymentDevicesLaunchCheckbox_Checked;
            deployPicker.Children.Add(deploymentDevicesLaunchCheckbox);
            Misc.Application.Lifecycle.Hardware81.SetLaunchAppAfterInstall(deploymentDevicesLaunchCheckbox.IsChecked.Value);

            Button deploymentDevicesButton = new Button
            {
                Content = Resources.Strings.AppStrings.DEPLOYMENT_DEPLOY_TO_SELECTED,
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 200
            };
            deploymentDevicesButton.Click += DeployAppsButton_Click;
            deployPicker.Children.Add(deploymentDevicesButton);
        }

        /// <summary>
        /// Change the selected device when the selection of the combobox changed.
        /// </summary>
        private void SelectedDevicesCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDeviceInfo = (Microsoft.Phone.Tools.Deploy.DeviceInfo)((ComboBox)sender).SelectedItem;
        }

        /// <summary>
        /// When checked, applications should be launched after installation.
        /// </summary>
        private void DeploymentDevicesLaunchCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            bool launchAfterInstall = ((CheckBox)sender).IsChecked.Value;
            Misc.Application.Lifecycle.Hardware81.SetLaunchAppAfterInstall(launchAfterInstall);
        }

        /// <summary>
        /// When clicked, deploy the applications to the selected device.
        /// </summary>
        private void DeployAppsButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDeviceInfo == devicesInfo[0] && Misc.Application.Lifecycle.DevicesUSB.Count == 0)
            {
                BitmapImage warningImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_WARNING, Misc.Image.Extensions.png);
                DialogPopup dialogPopup = new DialogPopup(AppStrings.MANAGER_WARNING, AppStrings.MANAGER_CONNECT_DEVICE, null, DialogPopup.DefaultButtons.Ok, warningImage, false);
                dialogPopup.Show();
                return;
            }

            if (Misc.Application.Lifecycle.DevicesUSB.Count > 1 && selectedDeviceInfo == devicesInfo[0])
            {
                BitmapImage warningImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_WARNING, Misc.Image.Extensions.png);
                DialogPopup dialogPopup = new DialogPopup(AppStrings.MANAGER_WARNING, AppStrings.MANAGER_DISCONNECT_DEVICES, null, DialogPopup.DefaultButtons.Ok, warningImage, false);
                dialogPopup.Show();
                return;
            }

            DeployApps(Misc.Application.Lifecycle.ManifestInfoList, Misc.Application.Lifecycle.Paths);
        }

        /// <summary>
        /// Deploy the applications to the selected device.
        /// </summary>
        /// <param name="appManifestList">Manifest information for the applications</param>
        /// <param name="paths">Paths list for the applications</param>
        private void DeployApps(Microsoft.Phone.Tools.Deploy.IAppManifestInfo[] appManifestList, String[] paths)
        {
            new Thread(() =>
            {

                Application.Current.Dispatcher.Invoke(() =>
                {
                    BitmapImage infoImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_TIP, Misc.Image.Extensions.png);
                    deployDialog = new DialogPopup(AppStrings.MANAGER_INFO, String.Empty, null, DialogPopup.DefaultButtons.None, infoImage, false);
                    deployDialog.Show();
                });


                for (int i = 0; i < appManifestList.Length; i++)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        String dialogText = AppStrings.MANAGER__INSTALLING_APPS + "\n" + AppStrings.MANAGER_INSTALLING_PROGRESS + (i + 1) + "/" + appManifestList.Length + "\n" + AppStrings.MANAGER_INSTALLING_APPLICATION + appManifestList[i].Name;
                        Console.WriteLine(dialogText);
                        deployDialog.EditText(dialogText);
                    });

                    Microsoft.Phone.Tools.Deploy.DeviceInfo deviceInfo = selectedDeviceInfo;
                    Microsoft.Phone.Tools.Deploy.DeploymentOptions deploymentOptions = selectedDeploymentOption;

                    try
                    {
                        Misc.Application.Lifecycle.Hardware81.DeployApplication(deviceInfo, paths[i], appManifestList[i], deploymentOptions);
                    }
                    catch (Exception exception)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            ShowErrorDialog(exception.Message);
                        }));
                    }
                }
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    deployDialog.Close();
                }));

            }).Start();
        }

        /// <summary>
        /// Update the textbox for the selected app path.
        /// </summary>
        private void AppDeployBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox xapBox = (TextBox)sender;
            xapPath = xapBox.Text;
        }

        /// <summary>
        /// Change the selected deployment option when the selection of the combobox changed.
        /// </summary>
        private void DeploymentOptionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Microsoft.Phone.Tools.Deploy.DeploymentOptions? deploymentOption = FromDeploymentOptionString(((ComboBox)sender).SelectedItem.ToString());
            if (deploymentOption != null)
                selectedDeploymentOption = (Microsoft.Phone.Tools.Deploy.DeploymentOptions)deploymentOption;
        }

        /// <summary>
        /// Select the deployer version (currently only the Windows Phone 8.1 deployer is supported).
        /// </summary>
        private void DeployerSelectorCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Deployer? deployer = FromDeployerString(((ComboBox)sender).SelectedItem.ToString());
            if (deployer != null)
                selectedDeployer = (Deployer)deployer;
        }

        /// <summary>
        /// Get the selected deployment option as a string.
        /// </summary>
        /// <returns>Deployment option as string.</returns>
        private String GetDeploymentOptionString(Microsoft.Phone.Tools.Deploy.DeploymentOptions deploymentOption)
        {
            return deploymentOption.ToString();
        }
        
        /// <summary>
        /// Convert a String to a deployment option.
        /// </summary>
        /// <param name="deploymentOption">String that represents the deployment option.</param>
        /// <returns>Enum type of the deployment option.</returns>
        private Microsoft.Phone.Tools.Deploy.DeploymentOptions? FromDeploymentOptionString(String deploymentOption)
        {
            Object value = Enum.Parse(typeof(Microsoft.Phone.Tools.Deploy.DeploymentOptions), deploymentOption);

            if (value != null)
                return (Microsoft.Phone.Tools.Deploy.DeploymentOptions)value;
            return null;
        }

        /// <summary>
        /// Get the selected deployer as a String.
        /// </summary>
        /// <param name="deployer">Deployer enum type.</param>
        /// <returns>String that represents the deployer option.</returns>
        private String GetDeployerString(Deployer deployer)
        {
            switch (deployer)
            {
                case Deployer.Latest:
                    return deployerLatest;
            }
            return String.Empty;
        }

        /// <summary>
        /// Get the selected deployer as an enum type.
        /// </summary>
        /// <param name="deployer">String that represents the deployer option.</param>
        /// <returns>Enum type of the deployer option.</returns>
        private Deployer? FromDeployerString(String deployer)
        {
            switch (deployer)
            {
                case deployerLatest:
                    return Deployer.Latest;
            }
            return null;
        }

        /// <summary>
        /// Enum of the deployer option.
        /// </summary>
        private enum Deployer
        {
            Latest
        }

        /// <summary>
        /// Show an error dialog for when deployment failed.
        /// </summary>
        /// <param name="error">Error to display.</param>
        private void ShowErrorDialog(String error)
        {
            BitmapImage errorImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_ERROR, Misc.Image.Extensions.png);
            DialogPopup dialogPopup = new DialogPopup(AppStrings.MANAGER_ERROR, AppStrings.MANAGER_MICROSOFT_DEPLOY_ERROR + error, null, DialogPopup.DefaultButtons.None, errorImage, false);
            dialogPopup.Show();
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
