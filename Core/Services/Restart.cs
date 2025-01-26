using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Command_Generator.Core.Services
{
    internal class Restart
    {
        public static async void RestartApp()
        {
            var restartResult = await CoreApplication.RequestRestartAsync("Restarting the app...");

            if (restartResult == AppRestartFailureReason.NotInForeground || restartResult == AppRestartFailureReason.Other)
            {
                var dialog = new MessageDialog("The app could not be restarted. Please try again later.", "Restart Failed");
                await dialog.ShowAsync();
            }
        }
    }
}
