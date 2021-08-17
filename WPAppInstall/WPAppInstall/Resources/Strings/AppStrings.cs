using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPAppInstall.Resources.Strings
{
    public static class AppStrings
    {
        public static readonly String APP_NAME = "WPAppInstall";
        public static readonly String APP_DESCRIPTION = "Deploy apps to your Windows Phone device.";
        public static readonly String APP_ICON = "WPIcon";
        public static readonly String APP_BRAND_LOGO = "WPBrand";
        public static readonly String APP_APP_LOGO = "WPStartApp";
        public static readonly String APP_PHONE = "WPPhone";
        public static readonly String APP_SEPERATOR = "WPSeperator";
        public static readonly String APP_PACK_RESOURCES_IMAGES = "pack://application:,,,/Resources/Images/";
        public static readonly String APP_PAGE_ABOUT_TITLE = "About";

        public static readonly String APP_DIALOG_WARNING = "WPDialogWarning";
        public static readonly String APP_DIALOG_ERROR = "WPDialogError";
        public static readonly String APP_DIALOG_TIP = "WPDialogTip";
        public static readonly String APP_DIALOG_CELLPHONE = "WPDialogCellPhone";

        public static readonly String APP_PAGE_DEVICES_TITLE = "Devices";
        public static readonly String APP_PAGE_DEPLOY_TITLE = "Deploy";
        public static readonly String APP_PAGE_SETTINGS_TITLE = "Settings";

        public static readonly String APP_DIALOG_OK = "Ok";

        public static readonly String APP_PAGE_START_ABOUT = "WPAppInstall allows application deployment to unlocked Windows Phone devices. Connect your device and you are good to go. \".xap\" files can be selected and deployed using the \"Deploy\" tab. The application preferences can be adjusted under the \"Settings\" tab.";
        public static readonly String APP_PAGE_START_GITHUB_HINT = "To acquire more information and view the source code, please visit the following GitHub repository:";

        public static readonly String APP_URL_GITHUB = "https://github.com/jetspiking/WPAppInstall";
        public static readonly String APP_URL_LINKEDIN = "https://www.linkedin.com/in/dustinhendriks/";

        public static readonly String APP_WINDOWS_PHONE_7_DEVICE = "Windows Phone 7";
        public static readonly String APP_WINDOWS_PHONE_8_10_DEVICE = "Windows Phone 8 / 10";
        public static readonly String APP_WINDOWS_PHONE_7_PID = "04EC";

        public static readonly String APP_COLOR_THEMES = "Themes";
        public static readonly String APP_RESIZABLE = "Resizable";
        public static readonly String APP_ALLOW_CHANGING_WINDOW_SIZE = "Allows changing window size";

        public static readonly String DEVICE_WPD_ICON_LOCATION = @"C:\ProgramData\Microsoft\WPD";

        public static readonly String HARDWARE_NOTIFICATION = "Hardware Notification";
        public static readonly String HARDWARE_CONNECTED_USB_DEVICE = "Connected USB-device.";
        public static readonly String HARDWARE_DISCONNECTED_USB_DEVICE = "Disconnected USB-device.";

        public static readonly String HARDWARE_USB_CAPTION = "Device";
        public static readonly String HARDWARE_USB_MANUFACTURER = "Manufacturer";
        public static readonly String HARDWARE_USB_DESCRIPTION = "Description";
        public static readonly String HARDWARE_USB_SERVICE = "Service";
        public static readonly String HARDWARE_USB_PNP_CLASS = "PNPClass";
        public static readonly String HARDWARE_USB_STATUS = "Status";
        public static readonly String HARDWARE_USB_VID = "VID";
        public static readonly String HARDWARE_USB_PID = "PID";
        public static readonly String HARDWARE_USB_GUID = "GUID";

        public static readonly String DEPLOY_DEPLOYER_VERSION = "Deployer Version";
        public static readonly String DEPLOYMENT_APPS = "Apps";
        public static readonly String DEPLOYMENT_OPTION = "Deployment Option";
        public static readonly String DEPLOYMENT_DEPLOY = "Deploy";
        public static readonly String DEPLOYMENT_LAUNCH_AFTER_INSTALLATION = "Launch after install";
        public static readonly String DEPLOYMENT_DEPLOY_TO_SELECTED = "Start";

        public static readonly String DEPLOY_SELECT_APPS = "Select Windows Phone Apps (deployment)";
        public static readonly String DEPLOY_WP70_XAP_EXTENSION = "App"; // .xap
        public static readonly String DEPLOY_WP81_XAP_EXTENSION = "App"; // .xap, .appx, .appxbundle

        public static readonly String MANAGER_WARNING = "Warning";
        public static readonly String MANAGER_ERROR = "Error";
        public static readonly String MANAGER_MICROSOFT_DEPLOY_ERROR = "Microsoft .dll phone error: ";
        public static readonly String MANAGER_INFO = "Info";
        public static readonly String MANAGER_INSTALLING_APPLICATION = "Installing application: ";
        public static readonly String MANAGER_INSTALLING_PROGRESS = "Installation progress: ";
        public static readonly String MANAGER_READING = "Reading data, please wait... \nThis message will close automatically.";
        public static readonly String MANAGER__INSTALLING_APPS = "Installing app(s), please wait... \nThis message will close automatically.";
        public static readonly String MANAGER_DISCONNECT_DEVICES = "Please disconnect all your Windows Phones, except your targeted device.";
        public static readonly String MANAGER_CONNECT_DEVICE = "Please connect your Windows Phone first.";
        public static readonly String MANAGER_READ = "Read";
        public static readonly String MANAGER_INSTALLED_APPS = "Installed Applications";
        public static readonly String MANAGER_DEVICE_INFORMATION = "Device Information";
        public static readonly String MANAGER_DEVICE_SPECIFICATIONS = "Device Specifications";

        public static readonly String APPS_BROWSE = "Browse";
        public static readonly String APPS_LIST = "Apps list";
        public static readonly String APPS_LIST_CLEAR = "Clear list";
    }
}
