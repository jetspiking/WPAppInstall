using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPAppInstall.Interfaces
{
    /// <summary>
    /// Interface to navigate to a different application page.
    /// </summary>

    public interface INavigator
    {
        void Navigate(Misc.AppPages appPage);
    }
}
