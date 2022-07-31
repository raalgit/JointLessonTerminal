using JLClient.Core.Settings;
using Microsoft.Toolkit.Uwp.Notifications;

namespace JLClient.Core.Notifications
{
    public class SystemToastManager
    {
        public void SendToast(string text)
        {
            var settings = UserSettings.GetInstance();

            if (settings.FullSupport)
            {
                new ToastContentBuilder()
                    .AddArgument("action", "viewConversation")
                    .AddText(text)
                    .Show();
            }
        }
    }
}
