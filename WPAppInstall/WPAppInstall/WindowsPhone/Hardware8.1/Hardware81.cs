using Microsoft.Phone.Tools.Common;
using Microsoft.Phone.Tools.Deploy;
using Microsoft.SmartDevice.MultiTargeting.Connectivity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPAppInstall.WindowsPhone.Hardware;

namespace WPAppInstall.WindowsPhone.Hardware8._1
{
    /// <summary>
    /// This class provides functions to receive information from one connected Windows Phone, or perform actions on it.
    /// </summary>
    public class Hardware81
    {
        private DeviceController deviceController = DeviceController.GetInstance();

        public Hardware81()
        {
        }

        /// <summary>
        /// Enable / disable launching an application immediately after installation.
        /// </summary>
        /// <param name="launchAppAfterInstall">True or false for respectively launching or not launching an application after installation.</param>
        public void SetLaunchAppAfterInstall(bool launchAppAfterInstall)
        {
            Microsoft.Phone.Tools.Deploy.GlobalOptions.LaunchAfterInstall = launchAppAfterInstall;
        }

        /// <summary>
        /// Scan for connectable devices.
        /// </summary>
        /// <returns>Collection of ConnectableDevice instances.</returns>
        public System.Collections.ObjectModel.Collection<ConnectableDevice> ScanDevices()
        {
            MultiTargetingConnectivity multiTargetingConnectivity = new MultiTargetingConnectivity(CultureInfo.CurrentUICulture.LCID);
            System.Collections.ObjectModel.Collection<ConnectableDevice> connectableDevices = multiTargetingConnectivity.GetConnectableDevices(true);
            return connectableDevices;
        }

        /// <summary>
        /// Connect to a device.
        /// </summary>
        /// <param name="connectableDevice">ConnectableDevice instance.</param>
        /// <returns>IDevice handle for device.</returns>
        public Microsoft.SmartDevice.Connectivity.Interface.IDevice ConnectToDevice(ConnectableDevice connectableDevice)
        {
            Microsoft.SmartDevice.Connectivity.Interface.IDevice deviceHandle = connectableDevice.Connect(true);
            return deviceHandle;
        }

        /// <summary>
        /// Disconnect from a device.
        /// </summary>
        /// <param name="deviceHandle">IDevice instance.</param>
        public void DisconnectFromDevice(Microsoft.SmartDevice.Connectivity.Interface.IDevice deviceHandle)
        {
            deviceHandle.Disconnect();
        }

        /// <summary>
        /// Get a file deployer from a device.
        /// </summary>
        /// <param name="deviceHandle">IDevice instance.</param>
        /// <returns>IFileDeployer handle for device.</returns>
        public Microsoft.SmartDevice.Connectivity.Interface.IFileDeployer GetFileDeployer(Microsoft.SmartDevice.Connectivity.Interface.IDevice deviceHandle)
        {
            return deviceHandle.GetFileDeployer();
        }

        /// <summary>
        /// Get a list of remote applications installed on the device.
        /// </summary>
        /// <param name="deviceHandle">IDevice instance.</param>
        /// <returns>Collection of IRemoteApplication instances.</returns>
        public System.Collections.ObjectModel.Collection<Microsoft.SmartDevice.Connectivity.Interface.IRemoteApplication> GetRemoteApplications(Microsoft.SmartDevice.Connectivity.Interface.IDevice deviceHandle)
        {
            return deviceHandle.GetInstalledApplications();
        }

        /// <summary>
        /// Get the system information from a device.
        /// </summary>
        /// <param name="deviceHandle">IDevice instance.</param>
        /// <returns>ISystemInfo instance.</returns>
        public Microsoft.SmartDevice.Connectivity.Interface.ISystemInfo GetSystemInfo(Microsoft.SmartDevice.Connectivity.Interface.IDevice deviceHandle)
        {
            return deviceHandle.GetSystemInfo();
        }

        /// <summary>
        /// Deploy an application to a connected Windows Phone device.
        /// </summary>
        /// <param name="deviceInfo">DeviceInfo object which specifies the type of device for installation (device and different types of emulators).</param>
        /// <param name="appPath">Path to application that should be installed.</param>
        /// <param name="info">ManifestInfo from the app that should be installed.</param>
        /// <param name="onComplete">Callback when app is deployed successfully / unsuccessfully.</param>
        /// <param name="deploymentOptions">Different types of deployment can be selected for different internal handling.</param>
        public void DeployApplication(DeviceInfo deviceInfo, String appPath, IAppManifestInfo info, DeploymentOptions deploymentOptions)
        {
            Microsoft.Phone.Tools.Deploy.Utils.InstallApplication(deviceInfo, info, deploymentOptions, appPath);
        }

    }
}
