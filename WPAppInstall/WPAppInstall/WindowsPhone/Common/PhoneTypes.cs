using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPAppInstall.WindowsPhone.Common
{
    /// <summary>
    /// This enum lists the kind of device in terms of functionality. WindowsPhone7 should be handed differently than WindowsPhone8or10.
    /// </summary>
    public enum PhoneTypes
    {
        WindowsPhone7,
        WindowsPhone8Or10
    }
}
