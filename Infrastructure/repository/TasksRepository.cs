using Domain.Entity;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public IEnumerable<Tasks> GetAll()
        {
            return _context.Tasks.ToList();
        }

        public Tasks GetById(int id)
        {
            return _context.Tasks.Find(id);
        } 
        public IEnumerable<Tasks> GetTaskByIdProject(int id)
        {
            var data = _context.Tasks.Where(a => a.projectId == id ).ToList();
            return data ;
        }

        public void Create(Tasks tasks)
        {
            _context.Tasks.Add(tasks);
            _context.SaveChanges();
        }

        public void Update(Tasks tasks)
        {
            _context.Tasks.Update(tasks);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var tasks = _context.Tasks.Find(id);
            _context.Tasks.Remove(tasks);
            _context.SaveChanges();
        }
    }
}
