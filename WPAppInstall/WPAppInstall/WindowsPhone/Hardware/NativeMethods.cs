using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WPAppInstall.WindowsPhone.Hardware
{
    /// <summary>
    /// Import a few functions from PhoneRegDll which are being used.
    /// </summary>

    public sealed class NativeMethods
    {
        [DllImport("PhoneRegDll.dll")]
        internal static extern uint GetWinPhone8Port(int port, out int portMapped);

        [DllImport("PhoneRegDll.dll")]
        internal static extern int USBMonitorStart(NativeMethods.CallBack onConnected, NativeMethods.CallBack onDisconnected);

        [DllImport("PhoneRegDll.dll")]
        internal static extern int USBMonitorStop();

        public delegate void CallBack([MarshalAs(UnmanagedType.LPWStr)] string name);
    }
}
