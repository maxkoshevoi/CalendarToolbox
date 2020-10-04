using CalendarToolbox.Helpers;
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

            ThemeHelper.SetAppTheme(App.Current.RequestedTheme);
            App.Current.RequestedThemeChanged += (_, e) => ThemeHelper.SetAppTheme(e.RequestedTheme);
        }
    }
}
