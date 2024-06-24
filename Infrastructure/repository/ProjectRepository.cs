using Domain.Entity;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace Infrastructure.repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Project.ToListAsync();
        }


        public async Task<Project> GetByIdAsync(int id)
        {
            return await _context.Project.FindAsync(id);
        }

        public async Task AddAsync(Project project)
        {
            _context.Project.Add(project);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _context.Project.FindAsync(id);
            if (project != null)
            {
                _context.Project.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
    }
}

