using Command_Generator.UI.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Command_Generator;
using System.Threading;
using System.Threading.Tasks;
using Command_Generator.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Command_Generator.UI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        public string LoadedLanguage = LocalizeHelper.GetLanguage();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (LoadedLanguage == "en-US")
            {
                LanguageRadioButtons.SelectedIndex = 0;
            }

            if (LoadedLanguage == "zh-Hans-CN")
            {
                LanguageRadioButtons.SelectedIndex = 1;
            }


            //(LanguageRadioButtons.Items.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentLanguage)).IsChecked = true;


            var currentTheme = ThemeHelper.RootTheme.ToString();
            (ThemeRadioButtons.Items.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme)).IsChecked = true;


        }


        private void OnThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is RadioButton selectItem)
            {
                ThemeHelper.RootTheme = ThemeHelper.GetEnum<ElementTheme>(selectItem.Tag.ToString());
            }
        }

        private void OnLanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is RadioButton selectItem)
            {
                LocalizeHelper.SetLanguage(selectItem.Tag.ToString());



                if (selectItem.Tag.ToString() != LoadedLanguage)

                {
                    var dialog = new ContentDialog
                    {
                        Title = "Restart Required",
                        Content = "The app needs to be restarted for the changes to take effect.",
                        PrimaryButtonText = "Restart",
                        PrimaryButtonStyle = (Style)Application.Current.Resources["AccentButtonStyle"],
                        CloseButtonText = "Later"
                    };

                    dialog.ShowAsync();



                    dialog.PrimaryButtonClick += async (s, args) =>
                    {
                        ToastNotificationHelper.SendNotification("Restart","App is restarting","");
                        Core.Services.Restart.RestartApp();
                    };
                }


            }
        }

    }
}
