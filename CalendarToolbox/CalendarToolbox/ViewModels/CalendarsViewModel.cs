using CalendarToolbox.Services;
using CalendarToolbox.Views;
using Plugin.Calendars.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CalendarToolbox.ViewModels
{
    public class CalendarsViewModel : BaseViewModel
    {
        private Calendar _selectedItem; 
        public IDataStore<Calendar> Calendars { get; } = DependencyService.Get<IDataStore<Calendar>>();

        public ObservableCollection<Calendar> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Calendar> ItemTapped { get; }

        public CalendarsViewModel()
        {
            Title = "Calendars";
            Items = new ObservableCollection<Calendar>();
            LoadItemsCommand = new Command(async () => await LoadItems());

            ItemTapped = new Command<Calendar>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task LoadItems()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await Calendars.GetAllAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;

            bool isHavePermissions = await GetPermissions();
            if (!isHavePermissions)
            {
                // await App.Current.MainPage.DisplayAlert("Calendar permission", "Unable to get calendar read/write permission.", "Ok");
                Environment.Exit(0);
            }
        }

        public Calendar SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewCalendarPage));
        }

        async void OnItemSelected(Calendar item)
        {
            if (item is null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(CalendarDetailPage)}?{nameof(CalendarDetailViewModel.CalendarId)}={item.ExternalID}");
        }

        private async Task<bool> GetPermissions()
        {
            PermissionStatus? readStatus = null;
            PermissionStatus? writeStatus = null;
            try
            {
                // Getting permissions
                readStatus = await Permissions.CheckStatusAsync<Permissions.CalendarRead>();
                writeStatus = await Permissions.CheckStatusAsync<Permissions.CalendarWrite>();
                if (readStatus != PermissionStatus.Granted || writeStatus != PermissionStatus.Granted)
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        readStatus = await Permissions.RequestAsync<Permissions.CalendarRead>();
                        if (readStatus == PermissionStatus.Granted)
                        {
                            writeStatus = await Permissions.RequestAsync<Permissions.CalendarWrite>();
                        }
                    });
                }

                if (readStatus != PermissionStatus.Granted || writeStatus != PermissionStatus.Granted)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}