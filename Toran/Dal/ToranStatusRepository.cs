using Microsoft.EntityFrameworkCore;
using Toran.Dal;
using Toran.Models;

namespace Toran.DAL
{
    public class ToranStatusRepository : IToranStatusRepository
    {
        private readonly BoiappContext _context;

        public ToranStatusRepository(BoiappContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToranStatus>> GetAllAsync()
        {
            return await _context.ToranStatuses.ToListAsync();
        }

        public async Task<ToranStatus?> GetByIdAsync(int id)
        {
            return await _context.ToranStatuses.FindAsync(id);
        }

        public async Task AddAsync(ToranStatus toranStatus)
        {
            _context.ToranStatuses.Add(toranStatus);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(ToranStatus toranStatus)
        {
            _context.Entry(toranStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var toranStatus = await _context.ToranStatuses.FindAsync(id);
            if (toranStatus == null) return false;

            _context.ToranStatuses.Remove(toranStatus);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
