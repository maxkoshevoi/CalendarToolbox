using CalendarToolbox.ViewModels;
using Xamarin.Forms;

namespace CalendarToolbox.Views
{
    public partial class CalendarDetailPage : ContentPage
    {
        public CalendarDetailPage()
        {
            InitializeComponent();
            BindingContext = new CalendarDetailViewModel();
        }
    }
}