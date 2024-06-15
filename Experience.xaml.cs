using Command_Generator.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Command_Generator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Experience : Page
    {


        public Experience()
        {
            this.InitializeComponent();

        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var target in TargetServices.Targets)
                {
                    var found = splitText.All((key) =>
                    {
                        return target.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(target);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Target.Text = args.SelectedItem.ToString();
        }


        private async void ExperienceGenerateButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user inputs
            string commandType = ((RadioButton)CommandType.SelectedItem)?.Content?.ToString();
            string target = Target.Text;
            string amount = AmountTextBox.Text;
            string levelOrPoint = ((RadioButton)LevelOrPointComboBox.SelectedItem)?.Content?.ToString();

            string command = string.Empty;

            // Generate the command based on selected type
            if (commandType == "add" || commandType == "set")
            {
                command = $"experience {commandType} {target} {amount} {levelOrPoint}";
            }
            else if (commandType == "query")
            {
                command = $"experience query {target} {levelOrPoint}";
            }

            // Create the ContentDialog to display the command
            ContentDialog commandDialog = new ContentDialog
            {
                Title = "生成的命令",
                Content = new TextBlock
                {
                    Text = command,
                    TextWrapping = TextWrapping.Wrap
                },
                PrimaryButtonText = "确认并复制",
                PrimaryButtonStyle = (Style)Application.Current.Resources["AccentButtonStyle"],
                CloseButtonText = "取消"
            };
            // Handle the button clicks
            commandDialog.PrimaryButtonClick += (s, args) =>
            {
                CopyCommandToClipboard(command);
                ToastNotificationsServices toastServices = new ToastNotificationsServices();
                toastServices.ShowToastNotification("已复制命令: ", command, "ms-appx:///Assets/Clipboard.png");
            };

            // Show the ContentDialog
            await commandDialog.ShowAsync();
        }

        private void CopyCommandToClipboard(string command)
        {
            // Copy the command to the clipboard
            var dataPackage = new DataPackage();
            dataPackage.SetText(command);
            Clipboard.SetContent(dataPackage);
        }
}

}
