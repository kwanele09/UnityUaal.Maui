#if __ANDROID__

using Android.Content;
using UnityUaal.Maui;

namespace UnityUaalMaui.Unity
{
    public static partial class UnityBridge
    {
        public static void ShowMainWindow()
        {
            var intent = new Intent(Platform.CurrentActivity, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ReorderToFront | ActivityFlags.SingleTop);
            Platform.CurrentActivity.StartActivity(intent);
        }

        public static void ShowUnityWindow()
        {
            var intent = new Intent(Platform.CurrentActivity, typeof(UnityActivity));
            intent.AddFlags(ActivityFlags.ReorderToFront);
            Platform.CurrentActivity.StartActivity(intent);
        }
    }
}

#endif