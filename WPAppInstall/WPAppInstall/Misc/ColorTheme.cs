using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPAppInstall.Misc
{
    /// <summary>
    /// This class lists different color themes included in the application.
    /// </summary>
    public static class ColorTheme
    {
        private static readonly Color apple = Color.FromRgb(228, 13, 0);
        private static readonly Color banana = Color.FromRgb(228, 210, 0);
        private static readonly Color blackberry = Color.FromRgb(75, 75, 75);
        private static readonly Color blueberry = Color.FromRgb(0, 49, 228);
        private static readonly Color raspberry = Color.FromRgb(228, 0, 147);
        private static readonly Color lime = Color.FromRgb(19, 228, 0);
        private static readonly Color mango = Color.FromRgb(228, 138, 0);
        private static readonly Color plum = Color.FromRgb(149, 0, 228);
        private static readonly Color coconut = Color.FromRgb(0, 0, 0);

        /// <summary>
        /// Get the application theme as a color.
        /// </summary>
        /// <param name="theme">Selected application theme.</param>
        /// <returns>Color corresponding with the theme parsed as argument.</returns>
        public static Color GetThemeColor(Themes theme)
        {
            switch (theme)
            {
                case Themes.Apple: return apple;
                case Themes.Banana: return banana;
                case Themes.Blackberry: return blackberry;
                case Themes.Blueberry: return blueberry;
                case Themes.Raspberry: return raspberry;
                case Themes.Lime: return lime;
                case Themes.Mango: return mango;
                case Themes.Plum: return plum;
                case Themes.Coconut: return coconut;
                default: return blackberry;
            }
        }
    }
}
