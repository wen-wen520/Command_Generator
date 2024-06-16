using System;
using Windows.UI.Xaml.Controls;

namespace Command_Generator
{
    public sealed partial class DiscoverTipsDialog : ContentDialog
    {
        public DiscoverTipsDialog()
        {
            this.InitializeComponent();
        }

        private async void  ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Navigate to the URL
            var uri = new Uri("https://www.microsoft.com/windows/tips");
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Close the dialog
            this.Hide();
        }

        private void ThumbsUpButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Handle thumbs up click
        }

        private void ThumbsDownButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Handle thumbs down click
        }
    }
}
