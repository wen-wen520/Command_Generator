using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;


namespace Command_Generator
{
    public sealed partial class ToastNotificationsServices
    {
        public void ShowToastNotification(string title, string copy_text, string image_path)
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