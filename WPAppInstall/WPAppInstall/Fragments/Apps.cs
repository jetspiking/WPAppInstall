using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPAppInstall.Interfaces;
using WPAppInstall.Resources.Strings;

namespace WPAppInstall.Fragments
{
    /// <summary>
    /// This class builds the view / window to browse and display apps. 
    /// </summary>

    public class Apps : IFragment
    {
        private readonly StackPanel _rootPanel = new StackPanel();
        private readonly String _wp81FileExtension = "{0}|*.xap;*.appx;*.appxbundle";
        private Grid _applicationsGrid = new Grid();
        private int _applicationGridRowIndex = 0;

        public Apps()
        {
            // Browse Apps Section
            TextBlock deploymentAppsTextblock = new TextBlock
            {
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Text = Resources.Strings.AppStrings.DEPLOYMENT_APPS
            };

            Label deploymentAppsLabel = new Label
            {
                Content = deploymentAppsTextblock
            };
            _rootPanel.Children.Add(deploymentAppsLabel);

            Button deploymentAppsButton = new Button
            {
                Content = AppStrings.APPS_BROWSE,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            deploymentAppsButton.Click += DeploymentAppsButton_Click;
            _rootPanel.Children.Add(deploymentAppsButton);

            // Apps List
            TextBlock appListTextblock = new TextBlock
            {
                FontFamily = new FontFamily(Misc.Application.Text.FONT),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Text = AppStrings.APPS_LIST
            };

            Label appListLabel = new Label
            {
                Content = appListTextblock
            };
            _rootPanel.Children.Add(appListLabel);

            Button appListButton = new Button
            {
                Content = AppStrings.APPS_LIST_CLEAR,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            appListButton.Click += AppListButton_Click;
            _rootPanel.Children.Add(appListButton);

            ColumnDefinition appName = new ColumnDefinition();
            ColumnDefinition appPath = new ColumnDefinition();
            ColumnDefinition appGuid = new ColumnDefinition();

            _applicationsGrid.ColumnDefinitions.Add(appName);
            //_applicationsGrid.ColumnDefinitions.Add(appPath);
            _applicationsGrid.ColumnDefinitions.Add(appGuid);

            _applicationsGrid.Background = new SolidColorBrush(Colors.Black);
            _rootPanel.Children.Add(_applicationsGrid);
            

            if (Misc.Application.Lifecycle.paths.Length > 0) FillInfo();
        }

        private void AppListButton_Click(object sender, RoutedEventArgs e)
        {
            _applicationsGrid.Children.Clear();
            _applicationGridRowIndex = 0;
            Misc.Application.Lifecycle.manifestInfoList = new Microsoft.Phone.Tools.Deploy.IAppManifestInfo[0];
            Misc.Application.Lifecycle.paths = new String[0];
        }

        private void DeploymentAppsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFilePicker();
        }

        public StackPanel GetRoot()
        {
            return _rootPanel;
        }

        private void FillInfo()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                _applicationsGrid.Children.Clear();
                _applicationGridRowIndex = 0;

                for (int i = 0; i < Misc.Application.Lifecycle.manifestInfoList.Length; i++)
                {
                    RowDefinition row = new RowDefinition();
                    _applicationsGrid.RowDefinitions.Add(row);

                    Label appName = new Label
                    {
                        Content = Misc.Application.Lifecycle.manifestInfoList[i].Name
                    };

                    Label appPath = new Label
                    {
                        Content = Misc.Application.Lifecycle.paths[i].ToString()
                    };

                    Label appGuid = new Label
                    {
                        Content = Misc.Application.Lifecycle.manifestInfoList[i].ProductId.ToString()
                    };

                    _applicationsGrid.Children.Add(appName);
                    //_applicationsGrid.Children.Add(appPath);
                    _applicationsGrid.Children.Add(appGuid);

                    appName.Background = new SolidColorBrush(Colors.White);
                    appPath.Background = new SolidColorBrush(Colors.White);
                    appGuid.Background = new SolidColorBrush(Colors.White);

                    int topMargin = 0;
                    int bottomMargin = 0;

                    if (i==0)
                        topMargin = 1;
                    if (i==Misc.Application.Lifecycle.manifestInfoList.Length-1)
                        bottomMargin = 1;

                    appName.Margin = new Thickness(1, topMargin, 0, bottomMargin);
                    appPath.Margin = new Thickness(1, topMargin, 1, bottomMargin);
                    appGuid.Margin = new Thickness(0, topMargin, 1, bottomMargin);

                    appName.SetValue(Grid.ColumnProperty, 0);
                    appName.SetValue(Grid.RowProperty, _applicationGridRowIndex);

                    appPath.SetValue(Grid.ColumnProperty, 1);
                    appPath.SetValue(Grid.RowProperty, _applicationGridRowIndex);

                    appGuid.SetValue(Grid.ColumnProperty, 2);
                    appGuid.SetValue(Grid.RowProperty, _applicationGridRowIndex);

                    _applicationGridRowIndex++;
                }
            }));
        }

        private void OpenFilePicker()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = Resources.Strings.AppStrings.DEPLOY_SELECT_APPS,

                Filter = String.Format(_wp81FileExtension, Resources.Strings.AppStrings.DEPLOY_WP81_XAP_EXTENSION)
            };

            bool? flag = openFileDialog.ShowDialog();

            if (flag != null && flag.Value)
            {
                String[] paths = openFileDialog.FileNames;
                new Thread(() =>
                {
                    List<Microsoft.Phone.Tools.Deploy.IAppManifestInfo> manifestList = new List<Microsoft.Phone.Tools.Deploy.IAppManifestInfo>();
                    List<String> pathList = new List<String>();

                    for (int i = 0; i < paths.Length; i++)
                    {
                        try
                        {
                            manifestList.Add(Microsoft.Phone.Tools.Deploy.Utils.ReadAppManifestInfoFromPackage(paths[i]));
                            pathList.Add(paths[i]);
                        } catch(Exception exception)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                ShowErrorDialog(exception.Message);
                            }));
                        }
                    }
                    Misc.Application.Lifecycle.manifestInfoList = manifestList.ToArray();
                    Misc.Application.Lifecycle.paths = pathList.ToArray();

                    FillInfo();

                }).Start();


            }
        }

        private void ShowErrorDialog(String error)
        {
            BitmapImage errorImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_ERROR, Misc.Image.Extensions.png);
            DialogPopup dialogPopup = new DialogPopup(AppStrings.MANAGER_ERROR, AppStrings.MANAGER_MICROSOFT_DEPLOY_ERROR + error, null, DialogPopup.DefaultButtons.None, errorImage, false);
            dialogPopup.Show();
        }

    }
}
