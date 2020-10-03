using CalendarToolbox.Services;
using Plugin.Calendars.Abstractions;
using System;
using Xamarin.Forms;

namespace CalendarToolbox.ViewModels
{
    [QueryProperty(nameof(CalendarId), nameof(CalendarId))]
    public class NewCalendarViewModel : BaseViewModel
    {
        private string calendarId;
        public string CalendarId { get => calendarId; set => SetProperty(ref calendarId, value, OnCalendarIdChanged); }
        public IDataStore<Calendar> Calendars { get; } = DependencyService.Get<IDataStore<Calendar>>();

        private Calendar calendar = new Calendar();
        public Calendar Calendar { get => calendar; set => SetProperty(ref calendar, value); }
        public string CalendarName { get => calendar.Name; set => SetProperty(calendar.Name, value, () => calendar.Name = value); }

        public Command SaveCommand { get; set; }
        public Command CancelCommand { get; set; }

        public NewCalendarViewModel()
        {
            SaveCommand = new Command(OnSave, () => !string.IsNullOrWhiteSpace(Calendar.Name));
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));

            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private async void OnSave()
        {
            if (string.IsNullOrEmpty(CalendarId))
            {
                await Calendars.AddAsync(Calendar);
            }
            else
            {
                await Calendars.UpdateAsync(Calendar);
            }

            await Shell.Current.GoToAsync("..");
        }

        public async void OnCalendarIdChanged()
        {
            try
            {
                Calendar = await Calendars.GetAsync(CalendarId);
                OnPropertyChanged(nameof(CalendarName));
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Failed to Load Item", ex.ToString(), "Ok");
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
