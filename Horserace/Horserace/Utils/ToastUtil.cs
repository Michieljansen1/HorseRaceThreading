using System;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Horserace.Utlis
{
    /// <summary>
    ///     Manages the toast to inform the user
    /// </summary>
    class ToastUtil
    {
        /// <summary>
        ///     Builds and shows a toast
        /// </summary>
        /// <param name="title">Title that appears on the toast</param>
        /// <param name="content">Text inside the toast</param>
        public static void Notify(String title, String content)
        {
            // Building the toast layout 
            var visual = new ToastVisual {
                BindingGeneric = new ToastBindingGeneric {
                    Children = {new AdaptiveText {Text = title}, new AdaptiveText {Text = content}},
                    AppLogoOverride = new ToastGenericAppLogo {
                        Source = "Assets/Square44x44Logo.scale-400.png", HintCrop = ToastGenericAppLogoCrop.Circle
                    }
                }
            };

            // Adding toast content
            var toastContent = new ToastContent {Visual = visual};

            // Sending and displaying the notification
            var toast = new ToastNotification(toastContent.GetXml()) {Priority = ToastNotificationPriority.High};
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}