using System;
using CalendarToolbox.Helpers;
using CalendarToolbox.Services;
using CalendarToolbox.Views;
using Microsoft.AppCenter.Crashes;
using Plugin.Calendars.Abstractions;
using Xamarin.Forms;

namespace CalendarToolbox.ViewModels
{
    [QueryProperty(nameof(CalendarId), nameof(CalendarId))]
    public class CalendarDetailViewModel : BaseViewModel
    {
        public IDataStore<Calendar> Calendars { get; } = DependencyService.Get<IDataStore<Calendar>>();

        private string calendarId;
        public string CalendarId { get => calendarId; set { SetProperty(ref calendarId, value); OnCalendarIdChanged(); } }

        private Calendar calendar;
        public Calendar Calendar { get => calendar; set => SetProperty(ref calendar, value); }

        public Command EditCommand { get; }
        public Command DeleteCommand { get; }

        public CalendarDetailViewModel()
        {
            EditCommand = CommandHelper.Create(OpenEditCalendarPage, () => Calendar?.CanEditCalendar == true);
            DeleteCommand = CommandHelper.Create(DeleteCalendar, () => Calendar?.CanEditCalendar == true);
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
                Crashes.TrackError(ex);
                await Shell.Current.CurrentPage.DisplayAlert("Failed to load calendar", ex.ToString(), "Ok");
                await Shell.Current.GoToAsync("..");
                return;
            }

            EditCommand.ChangeCanExecute();
            DeleteCommand.ChangeCanExecute();
        }
    }
}
