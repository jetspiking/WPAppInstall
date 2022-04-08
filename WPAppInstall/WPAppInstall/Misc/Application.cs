using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPAppInstall.WindowsPhone.Hardware;
using WPAppInstall.WindowsPhone.Hardware8._1;

namespace WPAppInstall.Misc
{
    /// <summary>
    /// This class provides a few default settings and a Lifecycle class which contains instances of objects for easy access. 
    /// </summary>
    public static class Application
    {
        public static class Margin
        {
            public static readonly int SELECTED_PAGE_BAR_HEIGHT = 5;
        }

        public static class Text
        {
            public static readonly String FONT = "Segoe UI";
            public static readonly int LARGE_FONT_SIZE = 38;
        }

        public static class Generic
        {
            public static readonly int MAX_PAGE_HISTORY = 9;
        }

        /// <summary>
        /// Class that stores data relevant to the application in a lifecycle object.
        /// </summary>
        public static class Lifecycle
        {
            public static Themes ApplicationTheme = Themes.Blackberry;
            public static Color ApplicationColor = ColorTheme.GetThemeColor(ApplicationTheme);
            public static AppPages ApplicationPage = AppPages.Start;
            private static List<AppPages> applicationPageHistory = new List<AppPages>();
            public static Hardware81 Hardware81 = new Hardware81();
            public static List<USBScanner.USBDevice> DevicesUSB = USBScanner.GetUSBDevices(USBScanner.FilterPropertiesWindowsPhone);
            public static String[] Paths = new String[0];
            public static Microsoft.Phone.Tools.Deploy.IAppManifestInfo[] ManifestInfoList = new Microsoft.Phone.Tools.Deploy.IAppManifestInfo[0];

            /// <summary>
            /// Check whether the page history is empty or not.
            /// </summary>
            /// <returns></returns>
            public static Boolean HistoryContainsMorePages()
            {
                return applicationPageHistory.Count > 0;
            }

            /// <summary>
            /// Navigate to the previously visited page.
            /// </summary>
            /// <returns>Previously visited page.</returns>
            public static AppPages? PopPage()
            {
                if (applicationPageHistory.Count>0)
                {
                    AppPages appPage = applicationPageHistory[applicationPageHistory.Count - 1];
                    applicationPageHistory.Remove(appPage);
                    return appPage;
                }
                return null;
            }

            /// <summary>
            /// Add a page to the navigation history.
            /// </summary>
            /// <param name="page">Last visited page.</param>
            public static void AddPage(AppPages page)
            {
                applicationPageHistory.Add(page);
                if (applicationPageHistory.Count >= Generic.MAX_PAGE_HISTORY)
                    applicationPageHistory.RemoveAt(0);
            }
        }
    }
}
