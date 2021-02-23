using CalendarToolbox.Themes;
using CalendarToolbox.Views;
using Xamarin.Forms;

namespace CalendarToolbox
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CalendarDetailPage), typeof(CalendarDetailPage));
            Routing.RegisterRoute(nameof(NewCalendarPage), typeof(NewCalendarPage));
            InitTheme();
        }

        private static void InitTheme()
        {
            ThemeHelper.SetAppTheme(App.Current.RequestedTheme);
            App.Current.RequestedThemeChanged += (_, e) => ThemeHelper.SetAppTheme(e.RequestedTheme);
        }
    }
}
