using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading;
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
        private readonly StackPanel rootPanel = new StackPanel();
        private readonly String wp81FileExtension = "{0}|*.xap;*.appx;*.appxbundle";
        private Grid applicationsGrid = new Grid();
        private Int32 applicationGridRowIndex = 0;

        /// <summary>
        /// Build the view / window to browse and display apps.
        /// </summary>
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
            rootPanel.Children.Add(deploymentAppsLabel);

            Button deploymentAppsButton = new Button
            {
                Content = AppStrings.APPS_BROWSE,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            deploymentAppsButton.Click += DeploymentAppsButton_Click;
            rootPanel.Children.Add(deploymentAppsButton);

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
            rootPanel.Children.Add(appListLabel);

            Button appListButton = new Button
            {
                Content = AppStrings.APPS_LIST_CLEAR,
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            appListButton.Click += AppListButton_Click;
            rootPanel.Children.Add(appListButton);

            ColumnDefinition appName = new ColumnDefinition();
            ColumnDefinition appPath = new ColumnDefinition();
            ColumnDefinition appGuid = new ColumnDefinition();

            applicationsGrid.ColumnDefinitions.Add(appName);
            applicationsGrid.ColumnDefinitions.Add(appGuid);

            applicationsGrid.Background = new SolidColorBrush(Colors.Black);
            rootPanel.Children.Add(applicationsGrid);
            

            if (Misc.Application.Lifecycle.Paths.Length > 0) FillInfo();
        }

        /// <summary>
        /// On click clear and (re)load the list of apps.
        /// </summary>
        private void AppListButton_Click(object sender, RoutedEventArgs e)
        {
            applicationsGrid.Children.Clear();
            applicationGridRowIndex = 0;
            Misc.Application.Lifecycle.ManifestInfoList = new Microsoft.Phone.Tools.Deploy.IAppManifestInfo[0];
            Misc.Application.Lifecycle.Paths = new String[0];
        }

        /// <summary>
        /// Open a file picker to select the applications to deploy.
        /// </summary>
        private void DeploymentAppsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFilePicker();
        }

        /// <summary>
        /// Get the root of this view for displaying purposes.
        /// </summary>
        /// <returns>Root of this view.</returns>
        public StackPanel GetRoot()
        {
            return rootPanel;
        }

        /// <summary>
        /// Fill the application grid.
        /// </summary>
        private void FillInfo()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                applicationsGrid.Children.Clear();
                applicationGridRowIndex = 0;

                for (int i = 0; i < Misc.Application.Lifecycle.ManifestInfoList.Length; i++)
                {
                    RowDefinition row = new RowDefinition();
                    applicationsGrid.RowDefinitions.Add(row);

                    Label appName = new Label
                    {
                        Content = Misc.Application.Lifecycle.ManifestInfoList[i].Name
                    };

                    Label appPath = new Label
                    {
                        Content = Misc.Application.Lifecycle.Paths[i].ToString()
                    };

                    Label appGuid = new Label
                    {
                        Content = Misc.Application.Lifecycle.ManifestInfoList[i].ProductId.ToString()
                    };

                    applicationsGrid.Children.Add(appName);
                    applicationsGrid.Children.Add(appGuid);

                    appName.Background = new SolidColorBrush(Colors.White);
                    appPath.Background = new SolidColorBrush(Colors.White);
                    appGuid.Background = new SolidColorBrush(Colors.White);

                    int topMargin = 0;
                    int bottomMargin = 0;

                    if (i==0)
                        topMargin = 1;
                    if (i==Misc.Application.Lifecycle.ManifestInfoList.Length-1)
                        bottomMargin = 1;

                    appName.Margin = new Thickness(1, topMargin, 0, bottomMargin);
                    appPath.Margin = new Thickness(1, topMargin, 1, bottomMargin);
                    appGuid.Margin = new Thickness(0, topMargin, 1, bottomMargin);

                    appName.SetValue(Grid.ColumnProperty, 0);
                    appName.SetValue(Grid.RowProperty, applicationGridRowIndex);

                    appPath.SetValue(Grid.ColumnProperty, 1);
                    appPath.SetValue(Grid.RowProperty, applicationGridRowIndex);

                    appGuid.SetValue(Grid.ColumnProperty, 2);
                    appGuid.SetValue(Grid.RowProperty, applicationGridRowIndex);

                    applicationGridRowIndex++;
                }
            }));
        }

        /// <summary>
        /// Open a file picker to select the applications to deploy.
        /// </summary>
        private void OpenFilePicker()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = Resources.Strings.AppStrings.DEPLOY_SELECT_APPS,
                Filter = String.Format(wp81FileExtension, Resources.Strings.AppStrings.DEPLOY_WP81_XAP_EXTENSION)
            };

            Boolean? flag = openFileDialog.ShowDialog();

            if (flag != null && flag.Value)
            {
                String[] paths = openFileDialog.FileNames;
                new Thread(() =>
                {
                    List<Microsoft.Phone.Tools.Deploy.IAppManifestInfo> manifestList = new List<Microsoft.Phone.Tools.Deploy.IAppManifestInfo>();
                    List<String> pathList = new List<String>();

                    for (Int32 i = 0; i < paths.Length; i++)
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
                    Misc.Application.Lifecycle.ManifestInfoList = manifestList.ToArray();
                    Misc.Application.Lifecycle.Paths = pathList.ToArray();

                    FillInfo();

                }).Start();
            }
        }

        /// <summary>
        /// Show an error dialog for when reading the applications fails.
        /// </summary>
        /// <param name="error">Error to display.</param>
        private void ShowErrorDialog(String error)
        {
            BitmapImage errorImage = Misc.Image.GetResourceImage(AppStrings.APP_DIALOG_ERROR, Misc.Image.Extensions.png);
            DialogPopup dialogPopup = new DialogPopup(AppStrings.MANAGER_ERROR, AppStrings.MANAGER_MICROSOFT_DEPLOY_ERROR + error, null, DialogPopup.DefaultButtons.None, errorImage, false);
            dialogPopup.Show();
        }

    }
}
