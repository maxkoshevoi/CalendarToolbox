using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalendarToolbox.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(string id);
        Task<T> GetAsync(string id, bool forceRefresh = false);
        Task<IEnumerable<T>> GetAllAsync(bool forceRefresh = false);
    }
}
