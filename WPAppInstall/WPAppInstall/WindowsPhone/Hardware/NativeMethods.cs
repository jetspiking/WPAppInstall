using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WPAppInstall.WindowsPhone.Hardware
{
    /// <summary>
    /// Import a functions from PhoneRegDll which are being used.
    /// </summary>
    public sealed class NativeMethods
    {
        [DllImport("PhoneRegDll.dll")]
        internal static extern uint GetWinPhone8Port(Int32 port, out Int32 portMapped);

        [DllImport("PhoneRegDll.dll")]
        internal static extern int USBMonitorStart(NativeMethods.CallBack onConnected, NativeMethods.CallBack onDisconnected);

        [DllImport("PhoneRegDll.dll")]
        internal static extern int USBMonitorStop();

        public delegate void CallBack([MarshalAs(UnmanagedType.LPWStr)] String name);
    }
}
