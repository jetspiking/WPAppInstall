using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPAppInstall.Interfaces;
using WPAppInstall.Resources.Strings;

namespace WPAppInstall.Fragments
{
    /// <summary>
    /// This class builds the view / window to adjust the app settings. 
    /// </summary>
    public class Settings : IFragment
    {
        private readonly StackPanel rootPanel = new StackPanel();
        private readonly ComboBox colorPreference = new ComboBox();
        private readonly IThemeUpdatable themeUpdatable;
        private readonly IResizable resizable;

        /// <summary>
        /// Build the view or page that is used for changing the application settings.
        /// </summary>
        /// <param name="themeUpdatable">Callback for updating the application theme.</param>
        /// <param name="resizable">Callback for updating the application resize settings.</param>
        public Settings(IThemeUpdatable themeUpdatable, IResizable resizable)
        {
            this.themeUpdatable = themeUpdatable;
            this.resizable = resizable;

            Label colorLabel = new Label
            {
                Content = Resources.Strings.AppStrings.APP_COLOR_THEMES,
                FontFamily = new System.Windows.Media.FontFamily(Misc.Application.Text.FONT)
            };

            foreach (Misc.Themes theme in (Misc.Themes[])Enum.GetValues(typeof(Misc.Themes)))
            {
                colorPreference.Items.Add(theme);
                colorPreference.SelectionChanged += ColorPreference_SelectionChanged;
            }

            colorPreference.SelectedItem = Misc.Application.Lifecycle.ApplicationTheme;

            rootPanel.Children.Add(colorLabel);
            rootPanel.Children.Add(colorPreference);

            Label resizableLabel = new Label
            {
                Content = Resources.Strings.AppStrings.APP_RESIZABLE,
                FontFamily = new System.Windows.Media.FontFamily(Misc.Application.Text.FONT)
            };

            CheckBox resizableCheckbox = new CheckBox
            {
                Content = AppStrings.APP_ALLOW_CHANGING_WINDOW_SIZE
            };
            resizableCheckbox.Click += ResizableCheckbox_Click;

            rootPanel.Children.Add(resizableLabel);
            rootPanel.Children.Add(resizableCheckbox);
        }

        /// <summary>
        /// Change the application resize mode.
        /// </summary>
        private void ResizableCheckbox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            resizable.SetResizable(checkBox.IsChecked.Value);
        }

        /// <summary>
        /// Update the application theme color.
        /// </summary>
        private void ColorPreference_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Misc.Application.Lifecycle.ApplicationTheme = (Misc.Themes)colorPreference.SelectedItem;
            Misc.Application.Lifecycle.ApplicationColor = Misc.ColorTheme.GetThemeColor(Misc.Application.Lifecycle.ApplicationTheme);
            themeUpdatable.UpdateAppTheme();
        }

        /// <summary>
        /// Get the root of this view for displaying purposes.
        /// </summary>
        /// <returns>Root of this view.</returns>
        public StackPanel GetRoot()
        {
            return rootPanel;
        }
    }
}
