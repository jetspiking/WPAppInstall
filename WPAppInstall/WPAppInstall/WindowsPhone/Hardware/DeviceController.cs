using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Win32.SafeHandles;
using System.Security;
using System.Runtime.ConstrainedExecution;
using System.Management;
using System.IO;

namespace WPAppInstall.WindowsPhone.Hardware
{
    /// <summary>
    /// This class allows to perform actions on Win32 devices.
    /// </summary>
    public class DeviceController
    {
        private const String cmdProcess = "cmd.exe";
        private const String enableDeviceCommand = "enable-device";
        private const String disableDeviceCommand = "disable-device";

        /// <summary>
        /// Function to disable file system redirection.
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Int32 Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        
        /// <summary>
        /// Initialize and disable file system redirection.
        /// </summary>
        private DeviceController()
        {
            IntPtr val = IntPtr.Zero;
            Wow64DisableWow64FsRedirection(ref val);
        }

        /// <summary>
        /// Get the DeviceController object singleton.
        /// </summary>
        /// <returns>DeviceController object singleton.</returns>
        public static DeviceController GetInstance()
        {
            return new DeviceController();
        }

        /// <summary>
        /// Enable or disable a device by the pnp device id.
        /// </summary>
        /// <param name="pnpDeviceId">Identifier for the device.</param>
        /// <param name="enable">Whether the device should be enabled or disabled.</param>
        public void EnableDevice(String pnpDeviceId, Boolean enable)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                FileName = cmdProcess,
                UseShellExecute = true,
                RedirectStandardOutput = false,
                Verb = "runas"
            };

            String command = enable ? enableDeviceCommand : disableDeviceCommand;
            startInfo.Arguments = $"/C \"pnputil /{command} \"{pnpDeviceId}\"\"";

            process.StartInfo = startInfo;
            process.Start();
        }
    }
}