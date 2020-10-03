using CalendarToolbox.ViewModels;
using Xamarin.Forms;

namespace CalendarToolbox.Views
{
    public partial class CalendarsPage : ContentPage
    {
        CalendarsViewModel _viewModel;

        public CalendarsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CalendarsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}