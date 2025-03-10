using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Windows.Devices.Pwm;
using Windows.Media.Capture.Core;
using Windows.ApplicationModel.Resources;


namespace Command_Generator.Services
{
    public sealed partial class ToastNotificationHelper
    {
        public static void SendNotification(string title = "", string copy_text = "", string image_path = "ms-appx:///Assets/Information.png")
        {
            // Create the toast notification content
            string toastXmlString = $@"
        <toast>
            <visual>
                <binding template='ToastGeneric'>
                    <image placement='appLogoOverride' src='{image_path}' alt='Image'/>
                    <text>{title}</text>
                    <text>{copy_text}</text>
                </binding>
            </visual>
        </toast>";

            var toastXml = new XmlDocument();
            toastXml.LoadXml(toastXmlString);

            // Create the toast notification and show it
            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}