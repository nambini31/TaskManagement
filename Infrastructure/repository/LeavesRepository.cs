using Domain.Entity;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class LeavesRepository : ILeavesRepository
    {
        private readonly ApplicationDbContext _context;

        public LeavesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Leaves>> GetAllAsync()
        {
            return await _context.Leaves.ToListAsync();
        }

        public async Task<Leaves> GetByIdAsync(int id)
        {
            return await _context.Leaves.FindAsync(id);
        }

        public async Task AddAsync(Leaves leaves)
        {
            _context.Leaves.Add(leaves);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Leaves leaves)
        {
            _context.Entry(leaves).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave != null)
            {
                _context.Leaves.Remove(leave);
                await _context.SaveChangesAsync();
            }
        }
    }
}
