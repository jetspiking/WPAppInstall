using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using WPAppInstall.Misc;

namespace WPAppInstall
{
    public partial class App : System.Windows.Application
    {
        private const String phoneRegDll = "PhoneRegDll.dll";

        /// <summary>
        /// Load embedded assemblies.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            EmbeddedAssemblyLoader embeddedAssemblyLoader = new EmbeddedAssemblyLoader();
            embeddedAssemblyLoader.LoadUnmanagedLibraryFromResource(phoneRegDll);
            StartProgram();
        }

        /// <summary>
        /// Called after loading the neccessary assemblies.
        /// </summary>
        public static void StartProgram()
        {
            var application = new App();
            application.InitializeComponent();
            application.Run();
        }
    }
}
