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
        public enum Extensions
        {
            png,
            jpg,
            jpeg
        }

        private static Uri GetImageUri(String imageName, String extension)
        {
            return new Uri($"{Resources.Strings.AppStrings.APP_PACK_RESOURCES_IMAGES}{imageName}.{extension}", UriKind.RelativeOrAbsolute);
        }

        private static BitmapImage GetImageByUri(Uri uri)
        {
            return new BitmapImage(uri);
        }

        public static BitmapFrame GetDeviceImage(String portableDeviceString)
        {
            String iconPath = $@"{Resources.Strings.AppStrings.DEVICE_WPD_ICON_LOCATION}\{portableDeviceString.Replace('\\', '#')}.ico";

            if (!File.Exists(iconPath)) return null; // return default Image.

            BitmapDecoder decoder = BitmapDecoder.Create(new Uri(iconPath), BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);
            BitmapFrame result = decoder.Frames.SingleOrDefault(f => f.Width == 256);
            return result;
        }


        public static BitmapImage GetResourceImage(String imageName, Extensions extension)
        {
            return GetImageByUri(GetImageUri(imageName.ToString(), extension.ToString()));
        }

        public static BitmapImage GetBrandLogo(VendorIds.Vendors vendor)
        {
            return GetImageByUri(GetImageUri(Resources.Strings.AppStrings.APP_BRAND_LOGO + vendor.ToString(), Extensions.png.ToString()));
        }

        public static BitmapImage GetAppImage(Start.Apps app)
        {
            return GetImageByUri(GetImageUri(Resources.Strings.AppStrings.APP_APP_LOGO + app.ToString(), Extensions.png.ToString()));
        }

        public static BitmapImage GetPhoneImage(Themes theme)
        {
            return GetImageByUri(GetImageUri(Resources.Strings.AppStrings.APP_PHONE + theme.ToString(), Extensions.png.ToString()));
        }

        public static BitmapImage GetIcon(Themes theme)
        {
            return GetImageByUri(GetImageUri(Resources.Strings.AppStrings.APP_ICON + theme.ToString(), Extensions.png.ToString()));
        }

        public static BitmapImage GetSeperator(Themes theme)
        {
            return GetImageByUri(GetImageUri(Resources.Strings.AppStrings.APP_SEPERATOR + theme.ToString(), Extensions.png.ToString()));
        }
    }
}
