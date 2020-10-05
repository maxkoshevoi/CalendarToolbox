using Android.Graphics;
using Android.OS;
using Android.Views;
using CalendarToolbox.Droid.Dependences;
using CalendarToolbox.Models.InterplatformCommunication;
using Plugin.CurrentActivity;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBarStyleManager))]
namespace CalendarToolbox.Droid.Dependences
{
    public class StatusBarStyleManager : IStatusBarStyleManager
    {
        public void SetColoredStatusBar(string hexColor)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.M)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var currentWindow = GetCurrentWindow();
                SetStatusBarIsLight(currentWindow, false);
                currentWindow.SetStatusBarColor(Color.ParseColor(hexColor));
                currentWindow.SetNavigationBarColor(Color.ParseColor(hexColor));
            });
        }

        public void SetWhiteStatusBar()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.M)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var currentWindow = GetCurrentWindow();
                SetStatusBarIsLight(currentWindow, true);
                currentWindow.SetStatusBarColor(Color.White);
                currentWindow.SetNavigationBarColor(Color.White);
            });
        }

        private static void SetStatusBarIsLight(Window currentWindow, bool isLight)
        {
            if ((int)Build.VERSION.SdkInt < 30)
            {
#pragma warning disable CS0618 // Type or member is obsolete. Using new API for Sdk 30+
                currentWindow.DecorView.SystemUiVisibility = isLight ? (StatusBarVisibility)(SystemUiFlags.LightStatusBar | SystemUiFlags.LightNavigationBar) : 0;
#pragma warning restore CS0618 // Type or member is obsolete
            }
            else
            {
                WindowInsetsControllerAppearance lightStatusBars = isLight ? WindowInsetsControllerAppearance.LightStatusBars | WindowInsetsControllerAppearance.LightNavigationBars : 0;
                currentWindow.InsetsController?.SetSystemBarsAppearance((int)lightStatusBars, (int)lightStatusBars);
            }
        }

        private Window GetCurrentWindow()
        {
            Window window = CrossCurrentActivity.Current.Activity.Window;
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            return window;
        }
    }
}