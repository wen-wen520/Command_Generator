using Command_Generator.UI.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Resources;
using System.Resources;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Command_Generator.UI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            TitleBarInitialize();

            ContentFrame.Navigate(typeof(HomePage));
            NavigationViewControl.Header = ResourceLoader.GetForCurrentView().GetString("HomePage_PageHeader");
        }
        private static void TitleBarInitialize()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = Windows.UI.Colors.Transparent;
            ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
        }

        private void NavigationViewControl_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var resourceLoader = ResourceLoader.GetForCurrentView();

            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
                NavigationViewControl.Header = resourceLoader.GetString("SettingsPage_PageHeader");
            }

            else if (args.InvokedItemContainer != null)
            {
                string invokedItemTag = args.InvokedItemContainer.Tag.ToString();
                Type pageType = Type.GetType($"Command_Generator.UI.Pages.{invokedItemTag}");


                if (pageType != null)
                {
                    ContentFrame.Navigate(pageType);
                    NavigationViewControl.Header = resourceLoader.GetString($"{(args.InvokedItemContainer as Microsoft.UI.Xaml.Controls.NavigationViewItem).Tag}_PageHeader");
                }
                else
                {
                }
            }
        }
    }
}