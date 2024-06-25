using Domain.Entity;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.repository
{
    public class TasksRepository : ITasksRepository
    {
        private readonly ApplicationDbContext _context;

        public TasksRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tasks>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<Tasks> GetByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task CreateAsync(Tasks tasks)
        {
            await _context.Tasks.AddAsync(tasks);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tasks tasks)
        {
            _context.Tasks.Update(tasks);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tasks = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(tasks);
            await _context.SaveChangesAsync();
        }
    }
}
