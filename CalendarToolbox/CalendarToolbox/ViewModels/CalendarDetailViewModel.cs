using System;
using System.Threading.Tasks;
using CalendarToolbox.Helpers;
using CalendarToolbox.Views;
using Microsoft.AppCenter.Crashes;
using Plugin.Calendars.Abstractions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace CalendarToolbox.ViewModels
{
    [QueryProperty(nameof(CalendarId), nameof(CalendarId))]
    public class CalendarDetailViewModel : BaseViewModel
    {
        private string calendarId;
        public string CalendarId { get => calendarId; set { SetProperty(ref calendarId, value, onChanged: async () => await OnCalendarIdChanged()); } }

        private Calendar calendar;
        public Calendar Calendar { get => calendar; set => SetProperty(ref calendar, value); }

        public IAsyncCommand EditCommand { get; }
        public IAsyncCommand DeleteCommand { get; }

        public CalendarDetailViewModel()
        {
            EditCommand = CommandHelper.Create(OpenEditCalendarPage, () => Calendar?.CanEditCalendar == true, allowsMultipleExecutions: false);
            DeleteCommand = CommandHelper.Create(DeleteCalendar, () => Calendar?.CanEditCalendar == true, allowsMultipleExecutions: false);
        }

        private async Task OpenEditCalendarPage()
        {
            await Shell.Current.GoToAsync($"{nameof(NewCalendarPage)}?{nameof(NewCalendarViewModel.CalendarId)}={Calendar.ExternalID}");
        }

        private async Task DeleteCalendar()
        {
            await Calendars.DeleteAsync(Calendar.ExternalID);
            await Shell.Current.GoToAsync("..");
        }

        public async Task OnCalendarIdChanged()
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

            EditCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }
    }
}
