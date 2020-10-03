using CalendarToolbox.ViewModels;
using Xamarin.Forms;

namespace CalendarToolbox.Views
{
    public partial class CalendarDetailPage : ContentPage
    {
        CalendarDetailViewModel _viewModel;

        public CalendarDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CalendarDetailViewModel();
        }

        private void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}