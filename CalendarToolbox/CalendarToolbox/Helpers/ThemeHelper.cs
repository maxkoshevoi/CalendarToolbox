using CalendarToolbox.Models.InterplatformCommunication;
using CalendarToolbox.Themes;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CalendarToolbox.Helpers
{
    class ThemeHelper
    {
        public static bool SetAppTheme(OSAppTheme selectedTheme)
        {
            var statusBarManager = DependencyService.Get<IStatusBarStyleManager>();
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries == null)
            {
                return false;
            }
            mergedDictionaries.Clear();

            switch (selectedTheme)
            {
                case OSAppTheme.Dark:
                    var theme = new DarkTheme();
                    mergedDictionaries.Add(theme);
                    statusBarManager.SetColoredStatusBar(((Color)theme["StatusBarColor"]).ToHex());
                    break;
                case OSAppTheme.Light:
                    mergedDictionaries.Add(new LightTheme());
                    statusBarManager.SetWhiteStatusBar();
                    break;
            }
            return true;
        }
    }
}
