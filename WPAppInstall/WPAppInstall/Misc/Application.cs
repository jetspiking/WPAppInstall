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

        public static class Lifecycle
        {
            public static Themes ApplicationTheme = Themes.Blackberry;
            public static Color ApplicationColor = ColorTheme.GetThemeColor(ApplicationTheme);
            public static AppPages ApplicationPage = AppPages.Start;
            private static List<AppPages> _applicationPageHistory = new List<AppPages>();
            public static Hardware81 Hardware81 = new Hardware81();
            public static List<USBScanner.USBDevice> DevicesUSB = USBScanner.GetUSBDevices(USBScanner.FILTER_PROPERTIES_WINDOWS_PHONE);
            public static String[] paths = new String[0];
            public static Microsoft.Phone.Tools.Deploy.IAppManifestInfo[] manifestInfoList = new Microsoft.Phone.Tools.Deploy.IAppManifestInfo[0];

            public static bool HistoryContainsMorePages()
            {
                return _applicationPageHistory.Count > 0;
            }

            public static AppPages? PopPage()
            {
                if (_applicationPageHistory.Count>0)
                {
                    AppPages appPage = _applicationPageHistory[_applicationPageHistory.Count - 1];
                    _applicationPageHistory.Remove(appPage);
                    return appPage;
                }
                return null;
            }

            public static void AddPage(AppPages page)
            {
                _applicationPageHistory.Add(page);
                if (_applicationPageHistory.Count >= Generic.MAX_PAGE_HISTORY)
                    _applicationPageHistory.RemoveAt(0);
            }
        }
    }
}
