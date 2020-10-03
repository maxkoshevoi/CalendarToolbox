using CalendarToolbox.ViewModels;
using Xamarin.Forms;

namespace CalendarToolbox.Views
{
    public partial class NewCalendarPage : ContentPage
    {
        public NewCalendarPage()
        {
            InitializeComponent();
            BindingContext = new NewCalendarViewModel();
        }
    }
}