﻿using CalendarToolbox.BL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calendars = Plugin.Calendars.Abstractions;

namespace CalendarToolbox.Services
{
    public class DeviceCalendarsDataStore : IDataStore<Calendars.Calendar>
    {
        private IList<Calendars.Calendar> items;

        public async Task<bool> AddAsync(Calendars.Calendar customCalendar)
        {
            await CalendarService.AddOrUpdateCalendarAsync(customCalendar);
            items.Add(customCalendar);

            return true;
        }

        public async Task<bool> UpdateAsync(Calendars.Calendar calendar)
        {
            await CalendarService.AddOrUpdateCalendarAsync(calendar);

            var oldItem = items.Where(c => c.ExternalID == calendar.ExternalID).Single();
            items.Remove(oldItem);
            items.Add(calendar);

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var oldItem = items.Where(c => c.ExternalID == id).Single();
            items.Remove(oldItem);

            await CalendarService.DeleteCalendarAsync(oldItem);

            return true;
        }

        public async Task<Calendars.Calendar> GetAsync(string id, bool forceRefresh = false)
        {
            await GetAllAsync(forceRefresh);
            return items.SingleOrDefault(s => s.ExternalID == id);
        }

        public async Task<IEnumerable<Calendars.Calendar>> GetAllAsync(bool forceRefresh = false)
        {
            if (forceRefresh || items == null)
            {
                items = await CalendarService.GetCalendarsAsync();
            }

            return items;
        }
    }
}