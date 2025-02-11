﻿using Domain.Entity;
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


        public async Task UpdateAsync(Project project, int user_maj)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Database.ExecuteSqlRaw("SET @userConnected = {0}", user_maj);

                    _context.Entry(project).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task DeleteAsync(int id, int user_maj)
        {
            var project = await _context.Project.FindAsync(id);

            if (project != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Database.ExecuteSqlRaw("SET @userConnected = {0}", user_maj);

                        _context.Project.Remove(project);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); throw ex;
                    }
                }
            }
        }
    }
}

