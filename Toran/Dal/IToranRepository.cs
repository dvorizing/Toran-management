using Toran.Models;

namespace Toran.Dal
{
    public interface IToranRepository
    {
        Task<IEnumerable<ToranModel>> GetAllAsync();
        Task<ToranModel?> GetByIdAsync(int id);
        Task AddAsync(ToranModel toran);
        Task<bool> UpdateAsync(ToranModel toran);
        Task<bool> DeleteAsync(int id);
    }
}
