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
        private const String CMD = "cmd.exe";
        private const String ENABLE_DEVICE_COMMAND = "enable-device";
        private const String DISABLE_DEVICE_COMMAND = "disable-device";

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        
        private DeviceController()
        {
            IntPtr val = IntPtr.Zero;
            Wow64DisableWow64FsRedirection(ref val);
        }

        public static DeviceController GetInstance()
        {
            return new DeviceController();
        }

        public void EnableDevice(String pnpDeviceId, bool enable)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                FileName = CMD,
                UseShellExecute = true,
                RedirectStandardOutput = false,
                Verb = "runas"
            };

            String command = enable ? ENABLE_DEVICE_COMMAND : DISABLE_DEVICE_COMMAND;
            startInfo.Arguments = $"/C \"pnputil /{command} \"{pnpDeviceId}\"\"";

            process.StartInfo = startInfo;
            process.Start();
        }
    }
}

// pnputil is a tool default installed on computers with Windows Vista and up, which allows for easy modification of USB-devices.
// Not working, attempt DevCon instead for removing a device, this isn't supported by pnputil. 