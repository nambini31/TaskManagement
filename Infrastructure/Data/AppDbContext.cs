using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        
        public DbSet<Leaves> Leaves { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
    }
}
