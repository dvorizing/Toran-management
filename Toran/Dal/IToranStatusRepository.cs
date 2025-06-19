using Toran.Models;

namespace Toran.Dal
{
    public interface IToranStatusRepository
    {
        Task<IEnumerable<ToranStatus>> GetAllAsync();
        Task<ToranStatus?> GetByIdAsync(int id);
        Task AddAsync(ToranStatus toranStatus);
        Task<bool> UpdateAsync(ToranStatus toranStatus);
        Task<bool> DeleteAsync(int id);
    }
}
