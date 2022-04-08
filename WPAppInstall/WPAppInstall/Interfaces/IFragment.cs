using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPAppInstall.Interfaces
{
    /// <summary>
    /// Interface for fragments.
    /// </summary>
    interface IFragment
    {
        StackPanel GetRoot();
    }
}
