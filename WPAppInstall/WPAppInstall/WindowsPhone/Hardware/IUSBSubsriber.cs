using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPAppInstall.WindowsPhone.Hardware
{
    /// <summary>
    /// Subscribe to USB-connected / USB-disconnected action.
    /// </summary>
    public interface IUSBSubsriber
    {
        void NotifyConnected(String name);
        void NotifyDisconnected(String name); 
    }
}
