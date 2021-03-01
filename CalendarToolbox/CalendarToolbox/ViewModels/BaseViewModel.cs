using CalendarToolbox.Services;
using Plugin.Calendars.Abstractions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace CalendarToolbox.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        bool isBusy = false;
        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }

        string title = string.Empty;
        public string Title { get => title; set => SetProperty(ref title, value); }

        protected IDataStore<Calendar> Calendars { get; } = DependencyService.Get<IDataStore<Calendar>>();
    }
}
