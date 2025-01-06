using Domain.DTO.ViewModels;
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
            //return await _context.Tasks.ToListAsync();
            return await _context.Tasks
            .Include(t => t.project)
            .ToListAsync();
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

        public async Task UpdateAsync(Tasks tasks, int user_maj)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //denifinition du session pour utiliser dans trigger
                    _context.Database.ExecuteSqlRaw("SET @userConnected = {0}", user_maj);

                    _context.Tasks.Update(tasks);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback(); 
                    throw ex;
                }
            }
        }

        public async Task DeleteAsync(int id, int user_maj)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //denifinition du session pour utiliser dans trigger
                    _context.Database.ExecuteSqlRaw("SET @userConnected = {0}", user_maj);

                    var tasks = await _context.Tasks.FindAsync(id);
                    _context.Tasks.Remove(tasks);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch(Exception) { transaction.Rollback(); throw; }
            }
        }
        public IEnumerable<Tasks> GetTaskByIdProject(int id)
        {
            var data = _context.Tasks.Where(a => a.projectId == id).ToList();
            return data;
        }
    }
}
