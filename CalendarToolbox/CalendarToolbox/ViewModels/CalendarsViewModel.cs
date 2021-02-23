using System;
using System.Linq;
using System.Threading.Tasks;
using CalendarToolbox.BL;
using CalendarToolbox.Helpers;
using CalendarToolbox.Services;
using CalendarToolbox.Views;
using Microsoft.AppCenter.Crashes;
using Plugin.Calendars.Abstractions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace CalendarToolbox.ViewModels
{
    public class CalendarsViewModel : BaseViewModel
    {
        public IDataStore<Calendar> Calendars { get; } = DependencyService.Get<IDataStore<Calendar>>();

        public ObservableRangeCollection<Calendar> Items { get; } = new();

        private Calendar _selectedItem;
        public Calendar SelectedItem { get => _selectedItem; set => SetProperty(ref _selectedItem, value, onChanged: () => OnItemSelected(value)); }

        public IAsyncCommand LoadItemsCommand { get; }
        public IAsyncCommand AddItemCommand { get; }
        public Command<Calendar> ItemTapped { get; }
        public Command PageAppearingCommand { get; }

        public CalendarsViewModel()
        {
            Title = "Calendars";
            PageAppearingCommand = CommandHelper.Create(OnAppearing);
            LoadItemsCommand = CommandHelper.Create(LoadItems);
            ItemTapped = CommandHelper.Create<Calendar>(OnItemSelected);
            AddItemCommand = CommandHelper.Create(async () => await Shell.Current.GoToAsync(nameof(NewCalendarPage)));
        }

        async Task LoadItems()
        {
            IsBusy = true;

            try
            {
                var calendars = await Calendars.GetAllAsync(true);
                Items.ReplaceRange(calendars
                    .OrderBy(c => !c.CanEditCalendar)
                    .ThenBy(c => c.Name)
                    .ThenBy(c => c.AccountName));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public bool isClosing = false;
        public async void OnAppearing()
        {
            if (isClosing)
            {
                return;
            }

            IsBusy = true;
            SelectedItem = null;

            bool isHavePermissions = await CalendarService.RequestPermissions();
            if (!isHavePermissions)
            {
                isClosing = true;
                await Shell.Current.CurrentPage.DisplayAlert("Calendar permission", "Unable to get calendar read/write permission.", "Ok");
                Environment.Exit(0);
            }
        }

        async void OnItemSelected(Calendar item)
        {
            if (item == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(CalendarDetailPage)}?{nameof(CalendarDetailViewModel.CalendarId)}={item.ExternalID}");
        }
    }
}