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
        }
    }
}
