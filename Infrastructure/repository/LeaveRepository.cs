using Domain.Entity;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.repository.LeaveRepository;

namespace Infrastructure.repository
{
    
        public class LeaveRepository : ILeaveRepository
        {
            private readonly AppDbContext _context;

            public LeaveRepository(AppDbContext context)
            {
                _context = context;
            }

            public async Task<List<Leave>> GetAllAsync()
            {
                return await _context.Leaves.ToListAsync();
            }

            public async Task<Leave> GetByIdAsync(int id)
            {
                return await _context.Leaves.FindAsync(id);
            }

            public async Task AddAsync(Leave leave)
            {
                await _context.Leaves.AddAsync(leave);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(Leave leave)
            {
                _context.Leaves.Update(leave);
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
