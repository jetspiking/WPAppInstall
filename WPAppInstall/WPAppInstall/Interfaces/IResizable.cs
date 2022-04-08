using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPAppInstall.Interfaces
{
    /// <summary>
    /// Interface to resize an application window.
    /// </summary>
    public interface IResizable
    {
        void SetResizable(bool enabled);
    }
}
