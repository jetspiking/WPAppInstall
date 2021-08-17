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
        private readonly StackPanel _rootPanel = new StackPanel();
        private readonly Label _startLabel = new Label();
        private readonly ComboBox _colorPreference = new ComboBox();
        private readonly IThemeUpdatable _themeUpdatable;
        private readonly IResizable _resizable;

        public Settings(IThemeUpdatable themeUpdatable, IResizable resizable)
        {
            _themeUpdatable = themeUpdatable;
            _resizable = resizable;

            Label colorLabel = new Label
            {
                Content = Resources.Strings.AppStrings.APP_COLOR_THEMES,
                FontFamily = new System.Windows.Media.FontFamily(Misc.Application.Text.FONT)
            };


            foreach (Misc.Themes theme in (Misc.Themes[])Enum.GetValues(typeof(Misc.Themes)))
            {
                _colorPreference.Items.Add(theme);
                _colorPreference.SelectionChanged += ColorPreference_SelectionChanged;
            }

            _colorPreference.SelectedItem = Misc.Application.Lifecycle.ApplicationTheme;

            _rootPanel.Children.Add(colorLabel);
            _rootPanel.Children.Add(_colorPreference);

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

            _rootPanel.Children.Add(resizableLabel);
            _rootPanel.Children.Add(resizableCheckbox);
        }

        private void ResizableCheckbox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            _resizable.SetResizable(checkBox.IsChecked.Value);
        }

        private void ColorPreference_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Misc.Application.Lifecycle.ApplicationTheme = (Misc.Themes)_colorPreference.SelectedItem;
            Misc.Application.Lifecycle.ApplicationColor = Misc.ColorTheme.GetThemeColor(Misc.Application.Lifecycle.ApplicationTheme);
            _themeUpdatable.UpdateAppTheme();
        }

        public StackPanel GetRoot()
        {
            return _rootPanel;
        }
    }
}
