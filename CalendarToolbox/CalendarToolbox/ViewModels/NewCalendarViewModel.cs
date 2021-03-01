using System;
using System.Threading.Tasks;
using CalendarToolbox.Helpers;
using Microsoft.AppCenter.Crashes;
using Plugin.Calendars.Abstractions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace CalendarToolbox.ViewModels
{
    [QueryProperty(nameof(CalendarId), nameof(CalendarId))]
    public class NewCalendarViewModel : BaseViewModel
    {
        private string calendarId;
        public string CalendarId { get => calendarId; set => SetProperty(ref calendarId, value, onChanged: async () => await OnCalendarIdChanged()); }
        private Calendar calendar = new();

        public string calendarName;
        public string CalendarName { get => calendarName; set => SetProperty(ref calendarName, value, onChanged: SaveCommand.RaiseCanExecuteChanged); }

        public IAsyncCommand SaveCommand { get; }
        public IAsyncCommand CancelCommand { get; }

        public NewCalendarViewModel()
        {
            SaveCommand = CommandHelper.Create(OnSave, () => !string.IsNullOrWhiteSpace(CalendarName), allowsMultipleExecutions: false);
            CancelCommand = CommandHelper.Create(async () => await Shell.Current.GoToAsync(".."));
        }

        private async Task OnSave()
        {
            calendar.Name = CalendarName;
            if (string.IsNullOrEmpty(CalendarId))
            {
                await Calendars.AddAsync(calendar);
            }
            else
            {
                await Calendars.UpdateAsync(calendar);
            }

            await Shell.Current.GoToAsync("..");
        }

        public async Task OnCalendarIdChanged()
        {
            try
            {
                calendar = await Calendars.GetAsync(CalendarId);
                CalendarName = calendar.Name;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await Shell.Current.CurrentPage.DisplayAlert("Failed to load calendar", ex.ToString(), "Ok");
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
