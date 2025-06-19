using Microsoft.EntityFrameworkCore;
using Toran.Dal;
using Toran.Models;

namespace Toran.DAL
{
    public class ToranRepository : IToranRepository
    {
        private readonly BoiappContext _context;

        public ToranRepository(BoiappContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToranModel>> GetAllAsync()
        {
            return await _context.Torans.ToListAsync();
        }

        public async Task<ToranModel?> GetByIdAsync(int id)
        {
            return await _context.Torans.FindAsync(id);
        }

        public async Task AddAsync(ToranModel toran)
        {
            _context.Torans.Add(toran);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(ToranModel toran)
        {
            _context.Entry(toran).State = EntityState.Modified;

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
            var toran = await _context.Torans.FindAsync(id);
            if (toran == null) return false;

            _context.Torans.Remove(toran);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
