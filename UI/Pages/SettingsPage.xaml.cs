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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var currentTheme = ThemeHelper.RootTheme.ToString();

            if (currentTheme == "Light")
            {
                ThemeRadioButtons.SelectedItem = LightThemeRadioButton;
            }

            if (currentTheme == "Dark")
            {
                ThemeRadioButtons.SelectedItem = DarkThemeRadioButton;
            }

            if (currentTheme == "Default")
            {
                ThemeRadioButtons.SelectedItem = SystemThemeRadioButton;
            }

        //    (ThemeRadioButtons.Items.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme)).IsChecked = true;
        }

        private void OnThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is RadioButton selectItem)
            {
                ThemeHelper.RootTheme = ThemeHelper.GetEnum<ElementTheme>(selectItem.Tag.ToString());
            }
        }

    }
}
