using Domain.Entity;
using Domain.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;


namespace Infrastructure.repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Project> GetAll()
        {
            return _context.Project.ToList();
        }

        public Project GetById(int id)
        {
            return _context.Project.Find(id);
        }

        public void Create(Project project)
        {
            _context.Project.Add(project);
            _context.SaveChanges();
        }

        public void Update(Project project)
        {
            _context.Project.Update(project);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var project = _context.Project.Find(id);
            _context.Project.Remove(project);
            _context.SaveChanges();
        }
    }
}

