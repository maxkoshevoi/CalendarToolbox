using System;
using CalendarToolbox.Helpers;
using CalendarToolbox.Services;
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
        public string CalendarId { get => calendarId; set => SetProperty(ref calendarId, value, onChanged: OnCalendarIdChanged); }
        private Calendar calendar = new();
        public IDataStore<Calendar> Calendars { get; } = DependencyService.Get<IDataStore<Calendar>>();

        public string calendarName;
        public string CalendarName { get => calendarName; set => SetProperty(ref calendarName, value, onChanged: SaveCommand.ChangeCanExecute); }

        public Command SaveCommand { get; }
        public IAsyncCommand CancelCommand { get; }

        public NewCalendarViewModel()
        {
            SaveCommand = CommandHelper.Create(OnSave, () => !string.IsNullOrWhiteSpace(CalendarName));
            CancelCommand = CommandHelper.Create(async () => await Shell.Current.GoToAsync(".."));
        }

        private async void OnSave()
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

        public async void OnCalendarIdChanged()
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
