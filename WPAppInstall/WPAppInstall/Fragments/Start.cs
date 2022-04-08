using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPAppInstall.Interfaces;

namespace WPAppInstall.Fragments
{
    /// <summary>
    /// This class builds the view / window to launch apps from the virtual phone selection menu (start app page). 
    /// </summary>
    public class Start : IFragment
    {
        private readonly StackPanel rootPanel = new StackPanel();
        private readonly ScrollViewer phoneScroller = new ScrollViewer();
        private readonly StackPanel aboutPanel = new StackPanel();
        private readonly Grid appGrid = new Grid();
        private readonly INavigator navigator;

        private readonly ButtonBoundary _buttonBackBoundary = new ButtonBoundary(new Point(41, 505), new Point(79, 541));
        private readonly ButtonBoundary _buttonStartBoundary = new ButtonBoundary(new Point(155, 505), new Point(193, 541));
        private readonly ButtonBoundary _buttonSearchBoundary = new ButtonBoundary(new Point(282, 505), new Point(312, 541));

        /// <summary>
        /// A button can have an x and y position in which an action is captured.
        /// </summary>
        internal class ButtonBoundary
        {
            public Point _x;
            public Point _y;

            public ButtonBoundary(Point x, Point y)
            {
                _x = x;
                _y = y;
            }
        }

        /// <summary>
        /// 'Applications' (or shortcuts) that are present by default in the app view.
        /// </summary>
        public enum Apps
        {
            About,
            Devices,
            Apps,
            Deploy,
            Manager,
            Settings,
            LinkedIn,
            Github
        }

        /// <summary>
        /// Build the view or page that is used as a starting page.
        /// </summary>
        /// <param name="navigator"></param>
        public Start(INavigator navigator)
        {
            this.navigator = navigator;

            // Init Scroll panel
            rootPanel.Orientation = Orientation.Vertical;
            rootPanel.Width = 350;
            rootPanel.Background = new ImageBrush(Misc.Image.GetPhoneImage(Misc.Application.Lifecycle.ApplicationTheme));
            rootPanel.MouseLeftButtonDown += RootPanel_MouseLeftButtonDown;
            phoneScroller.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            phoneScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            phoneScroller.Margin = new System.Windows.Thickness(20, 20, 20, 80);
            rootPanel.Children.Add(phoneScroller);

            // Init About panel
            Label aboutLabel = new Label
            {
                FontFamily = new System.Windows.Media.FontFamily(Misc.Application.Text.FONT),
                FontSize = Misc.Application.Text.LARGE_FONT_SIZE,
                Content = Resources.Strings.AppStrings.APP_PAGE_ABOUT_TITLE,
                Foreground = new SolidColorBrush(Misc.Application.Lifecycle.ApplicationColor)
            };

            aboutPanel.Children.Add(aboutLabel);

            TextBlock aboutHint = new TextBlock
            {
                Text = Resources.Strings.AppStrings.APP_PAGE_START_ABOUT,
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Margin = new System.Windows.Thickness(5, 0, 5, 0)
            };

            aboutPanel.Children.Add(aboutHint);

            TextBlock githubHint = new TextBlock
            {
                Margin = new System.Windows.Thickness(5, 15, 10, 5),
                Text = Resources.Strings.AppStrings.APP_PAGE_START_GITHUB_HINT,
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                TextWrapping = System.Windows.TextWrapping.Wrap
            };

            aboutPanel.Children.Add(githubHint);

            Button githubButton = new Button
            {
                Margin = new System.Windows.Thickness(5, 0, 5, 0),
                Content = Resources.Strings.AppStrings.APP_URL_GITHUB,
                Background = null,
                BorderThickness = new System.Windows.Thickness(0, 0, 0, 0),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                Foreground = new SolidColorBrush(Misc.ColorTheme.GetThemeColor(Misc.Themes.Blueberry))
            };
            githubButton.Click += GithubButton_Click;

            aboutPanel.Children.Add(githubButton);


            // Start panel
            appGrid = new Grid();
            appGrid.RowDefinitions.Add(new RowDefinition());
            appGrid.RowDefinitions.Add(new RowDefinition());
            appGrid.RowDefinitions.Add(new RowDefinition());
            appGrid.RowDefinitions.Add(new RowDefinition());
            appGrid.ColumnDefinitions.Add(new ColumnDefinition());
            appGrid.ColumnDefinitions.Add(new ColumnDefinition());

            phoneScroller.Content = appGrid;

            appGrid.Margin = new System.Windows.Thickness(20, 20, 20, 0);
            AddApp(appGrid, Apps.About, 0, 0);
            AddApp(appGrid, Apps.Devices, 1, 0);
            AddApp(appGrid, Apps.Apps, 0, 1);
            AddApp(appGrid, Apps.Deploy, 1, 1);
            AddApp(appGrid, Apps.Manager, 0, 2);
            AddApp(appGrid, Apps.Settings, 1, 2);
            AddApp(appGrid, Apps.Github, 0, 3);
            AddApp(appGrid, Apps.LinkedIn, 1, 3);

            phoneScroller.Content = appGrid;
        }

        /// <summary>
        /// Check whether a device button was clicked by verifying the known boundaries.
        /// </summary>
        private void RootPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition((StackPanel)sender);
            double x = mousePosition.X;
            double y = mousePosition.Y;

            if (x > _buttonBackBoundary._x.X && x < _buttonBackBoundary._y.X && y > _buttonBackBoundary._x.Y && y < _buttonBackBoundary._y.Y)
                phoneScroller.Content = appGrid;
            if (x > _buttonStartBoundary._x.X && x < _buttonStartBoundary._y.X && y > _buttonStartBoundary._x.Y && y < _buttonStartBoundary._y.Y)
                phoneScroller.Content = appGrid;
            if (x > _buttonSearchBoundary._x.X && x < _buttonSearchBoundary._y.X && y > _buttonSearchBoundary._x.Y && y < _buttonSearchBoundary._y.Y)
                phoneScroller.Content = appGrid;
        }

        /// <summary>
        /// Add an application to the grid.
        /// </summary>
        /// <param name="appGrid">Grid to add app to.</param>
        /// <param name="app">App to add to grid.</param>
        /// <param name="column">App column.</param>
        /// <param name="row">App row.</param>
        private void AddApp(Grid appGrid, Apps app, int column, int row)
        {
            StackPanel buttonStack = new StackPanel
            {
                Name = app.ToString()
            };
            buttonStack.MouseLeftButtonDown += ButtonStack_MouseLeftButtonDown;
            buttonStack.Background = new SolidColorBrush(Misc.ColorTheme.GetThemeColor(Misc.Application.Lifecycle.ApplicationTheme));
            buttonStack.Margin = new System.Windows.Thickness(5, 5, 5, 5);

            Image buttonImage = new Image
            {
                Width = 50,
                Height = 50,
                Source = Misc.Image.GetAppImage(app),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,

                Margin = new System.Windows.Thickness(5, 5, 5, 5)
            };

            Label appName = new Label
            {
                Content = app.ToString(),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                Foreground = new SolidColorBrush(Colors.White)
            };

            buttonStack.SetValue(Grid.ColumnProperty, column);
            buttonStack.SetValue(Grid.RowProperty, row);

            buttonStack.Children.Add(buttonImage);
            buttonStack.Children.Add(appName);

            appGrid.Children.Add(buttonStack);
        }

        /// <summary>
        /// Verify which app was pressed and perform an action.
        /// </summary>
        private void ButtonStack_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StackPanel stackPanel = (StackPanel)sender;
            switch (stackPanel.Name)
            {
                case nameof(Apps.About):
                    phoneScroller.Content = aboutPanel;
                    break;
                case nameof(Apps.Devices):
                    navigator.Navigate(Misc.AppPages.Devices);
                    break;
                case nameof(Apps.Apps):
                    navigator.Navigate(Misc.AppPages.Apps);
                    break;
                case nameof(Apps.Deploy):
                    navigator.Navigate(Misc.AppPages.Deploy);
                    break;
                case nameof(Apps.Manager):
                    navigator.Navigate(Misc.AppPages.Manager);
                    break;
                case nameof(Apps.Settings):
                    navigator.Navigate(Misc.AppPages.Settings);
                    break;
                case nameof(Apps.LinkedIn):
                    System.Diagnostics.Process.Start(Resources.Strings.AppStrings.APP_URL_LINKEDIN);
                    break;
                case nameof(Apps.Github):
                    System.Diagnostics.Process.Start(Resources.Strings.AppStrings.APP_URL_GITHUB);
                    break;
            }
        }

        /// <summary>
        /// This action is used for the separate link as displayed under the dedicated about section.
        /// </summary>
        private void GithubButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Resources.Strings.AppStrings.APP_URL_GITHUB);
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
