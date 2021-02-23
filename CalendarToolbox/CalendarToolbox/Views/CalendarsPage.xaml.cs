using CalendarToolbox.ViewModels;
using Xamarin.Forms;

namespace CalendarToolbox.Views
{
    public partial class CalendarsPage : ContentPage
    {
        public CalendarsPage()
        {
            InitializeComponent();
            BindingContext = new CalendarsViewModel();
        }
    }
}