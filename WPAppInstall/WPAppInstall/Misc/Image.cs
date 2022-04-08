using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPAppInstall.Fragments;
using WPAppInstall.WindowsPhone.Hardware;
using static WPAppInstall.WindowsPhone.Hardware.USBScanner.FilterProperties;

namespace WPAppInstall.Misc
{
    /// <summary>
    /// This class provides a few default functions for reading images out of the resource directory.
    /// </summary>

    public static class Image
    {
        /// <summary>
        /// Supported image extensions.
        /// </summary>
        public enum Extensions
        {
            png,
            jpg,
            jpeg
        }

        /// <summary>
        /// Get a uri for the desired image.
        /// </summary>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="extension">Extension of the image.</param>
        /// <returns></returns>
        private static Uri GetImageUri(String imageName, String extension)
        {
            return new Uri($"{Resources.Strings.AppStrings.APP_PACK_RESOURCES_IMAGES}{imageName}.{extension}", UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Get an image by a uri.
        /// </summary>
        /// <param name="uri">Image uri.</param>
        /// <returns>Image as BitmapImage.</returns>
        private static BitmapImage GetImageByUri(Uri uri)
        {
            return new BitmapImage(uri);
        }

        /// <summary>
        /// Get the image for a device.
        /// </summary>
        /// <param name="portableDeviceString">Device string for retrieving the image.</param>
        /// <returns>Device BitmapImage.</returns>
        public static BitmapFrame GetDeviceImage(String portableDeviceString)
        {
            String iconPath = $@"{Resources.Strings.AppStrings.DEVICE_WPD_ICON_LOCATION}\{portableDeviceString.Replace('\\', '#')}.ico";

            if (!File.Exists(iconPath)) return null;

            BitmapDecoder decoder = BitmapDecoder.Create(new Uri(iconPath), BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);
            BitmapFrame result = decoder.Frames.SingleOrDefault(f => f.Width == 256);
            return result;
        }

        /// <summary>
        /// Get a resource as BitmapImage.
        /// </summary>
        /// <param name="imageName">Name of the image.</param>
        /// <param name="extension">Extension of the image.</param>
        /// <returns></returns>
        public static BitmapImage GetResourceImage(String imageName, Extensions extension)
        {
            return GetImageByUri(GetImageUri(imageName.ToString(), extension.ToString()));
        }

        /// <summary>
        /// Get the brand as a BitmapImage.
        /// </summary>
        /// <param name="vendor">Vendor of the device.</param>
        /// <returns>Logo of the brand as BitmapImage.</returns>
        public static BitmapImage GetBrandLogo(VendorIds.Vendors vendor)
        {
            return GetImageByUri(GetImageUri(Resources.Strings.AppStrings.APP_BRAND_LOGO + vendor.ToString(), Extensions.png.ToString()));
        }

        /// <summary>
        /// Get an image for a default application (or shortcut) on the start screen.
        /// </summary>
        /// <param name="app">App to retrieve the image for.</param>
        /// <returns>BitmapImage for the app.</returns>
        public static BitmapImage GetAppImage(Start.Apps app)
        {
            return GetImageByUri(GetImageUri(Resources.Strings.AppStrings.APP_APP_LOGO + app.ToString(), Extensions.png.ToString()));
        }

        /// <summary>
        /// Get an image for the displayed phone on the start screen depending on the selected theme color.
        /// </summary>
        /// <param name="theme">Selected application theme color.</param>
        /// <returns>Start screen phone BitmapImage for the selected theme.</returns>
        public static BitmapImage GetPhoneImage(Themes theme)
        {
            return GetImageByUri(GetImageUri(Resources.Strings.AppStrings.APP_PHONE + theme.ToString(), Extensions.png.ToString()));
        }

        /// <summary>
        /// Get a logo image / icon for the application depending on the selected theme color.
        /// </summary>
        /// <param name="theme">Selected application theme color.</param>
        /// <returns>Start screen logo BitmapImage for the selected theme.</returns>
        public static BitmapImage GetIcon(Themes theme)
        {
            return GetImageByUri(GetImageUri(Resources.Strings.AppStrings.APP_ICON + theme.ToString(), Extensions.png.ToString()));
        }

        /// <summary>
        /// Get a seperator (vertical colored line) depending on the selected theme color.
        /// </summary>
        /// <param name="theme">Selected application theme color.</param>
        /// <returns>Start screen separator BitmapImage for the selected theme.</returns>
        public static BitmapImage GetSeperator(Themes theme)
        {
            return GetImageByUri(GetImageUri(Resources.Strings.AppStrings.APP_SEPERATOR + theme.ToString(), Extensions.png.ToString()));
        }
    }
}
