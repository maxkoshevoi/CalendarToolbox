using CalendarToolbox.Services;
using CalendarToolbox.Views;
using Plugin.Calendars.Abstractions;
using System;
using Xamarin.Forms;

namespace CalendarToolbox.ViewModels
{
    [QueryProperty(nameof(CalendarId), nameof(CalendarId))]
    public class CalendarDetailViewModel : BaseViewModel
    {
        private string calendarId;
        public string CalendarId 
        { 
            get => calendarId; 
            set 
            { 
                if (value != null)
                {
                    SetProperty(ref calendarId, value);
                }
            } 
        }
        public IDataStore<Calendar> Calendars { get; } = DependencyService.Get<IDataStore<Calendar>>();

        private Calendar calendar;
        public Calendar Calendar { get => calendar; set => SetProperty(ref calendar, value); }

        public Command EditCommand { get; set; }
        public Command DeleteCommand { get; set; }

        public CalendarDetailViewModel()
        {
            EditCommand = new Command(OpenEditCalendarPage, () => Calendar?.CanEditCalendar ?? false);
            DeleteCommand = new Command(DeleteCalendar, () => Calendar?.CanEditCalendar ?? false);
        }

        public void OnAppearing()
        {
            OnCalendarIdChanged();
        }

        private async void OpenEditCalendarPage()
        {
            await Shell.Current.GoToAsync($"{nameof(NewCalendarPage)}?{nameof(NewCalendarViewModel.CalendarId)}={Calendar.ExternalID}");
        }

        private async void DeleteCalendar()
        {
            await Calendars.DeleteAsync(Calendar.ExternalID);
            await Shell.Current.GoToAsync("..");
        }

        public async void OnCalendarIdChanged()
        {
            try
            {
                Calendar = null;
                Calendar = await Calendars.GetAsync(CalendarId);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Failed to Load Item", ex.ToString(), "Ok");
                await Shell.Current.GoToAsync("..");
                return;
            }

            EditCommand.ChangeCanExecute();
            DeleteCommand.ChangeCanExecute();
        }
    }
}
