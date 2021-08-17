using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPAppInstall.Resources.Strings;

namespace WPAppInstall
{
    /// <summary>
    /// Create a default application dialog popup.
    /// </summary>

    public partial class DialogPopup : Window
    {
        public enum DefaultButtons
        {
            Ok,
            None
        }

        public DialogPopup(String title, String content, BitmapSource image, DefaultButtons defaultButtons, BitmapSource deviceImage, bool isLongMessage=true)
        {
            InitializeComponent();

            this.Title = title;
            this.DialogImage.Source = image;
            this.DialogText.Text = content;
            this.DialogText.FontFamily = new FontFamily(Misc.Application.Text.FONT);
            this.DeviceImage.Source = deviceImage;
            this.Topmost = true;

            if (image == null) DialogImage.Height = 0;
            if (deviceImage == null) DeviceImage.Height = 0;

            if (defaultButtons != DefaultButtons.None)
                AddButtons(defaultButtons);
            else
                DeviceImage.Height = 125;

            if (!isLongMessage)
                 this.Height = 420;
            else this.Height = 535;
        }

        public void EditText(String content)
        {
            this.DialogText.Text = content;
        }

        private void AddButtons(DefaultButtons defaultButtons)
        {
            switch (defaultButtons)
            {
                case DefaultButtons.Ok:
                    Button defaultButton = new Button
                    {
                        Content = AppStrings.APP_DIALOG_OK,
                        FontFamily = new FontFamily(Misc.Application.Text.FONT),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = 100
                    };
                    defaultButton.Click += DefaultButton_Click;
                    DialogButtons.Children.Add(defaultButton);
                    break;
            }
        }

        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButtons(List<Button> defaultButtons)
        {
            defaultButtons.ForEach(button =>
            {
                DialogButtons.Children.Add(button);
            });
        }
    }
}
