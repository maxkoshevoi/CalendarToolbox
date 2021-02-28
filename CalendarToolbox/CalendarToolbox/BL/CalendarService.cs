using Plugin.Calendars;
using Plugin.Calendars.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CalendarToolbox.BL
{
    public static class CalendarService
    {
        public static async Task<bool> RequestPermissionsAsync()
        {
            PermissionStatus readStatus = await Permissions.CheckStatusAsync<Permissions.CalendarRead>();
            PermissionStatus writeStatus = await Permissions.CheckStatusAsync<Permissions.CalendarWrite>();
            if (readStatus != PermissionStatus.Granted)
            {
                readStatus = await Permissions.RequestAsync<Permissions.CalendarRead>();
            }
            if (writeStatus != PermissionStatus.Granted && readStatus == PermissionStatus.Granted)
            {
                writeStatus = await Permissions.RequestAsync<Permissions.CalendarWrite>();
            }
            return readStatus == PermissionStatus.Granted && writeStatus == PermissionStatus.Granted;
        }

        public static async Task<IList<Calendar>> GetCalendarsAsync()
        {
            if (!await RequestPermissionsAsync())
            {
                return new List<Calendar>();
            }

            IList<Calendar> calendars = await CrossCalendars.Current.GetCalendarsAsync();
            return calendars;
        }

        public static async Task AddOrUpdateCalendarAsync(Calendar calendar)
        {
            if (!await RequestPermissionsAsync())
            {
                return;
            }

            await CrossCalendars.Current.AddOrUpdateCalendarAsync(calendar);
        }


        public static async Task<bool> DeleteCalendarAsync(Calendar calendar)
        {
            if (!await RequestPermissionsAsync())
            {
                return false;
            }

            bool isRemoved = await CrossCalendars.Current.DeleteCalendarAsync(calendar);
            return isRemoved;
        }
    }
}
