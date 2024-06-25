﻿
using Microsoft.EntityFrameworkCore;
using Domain.Entity;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public int? CurrentUserId { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
  
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<UserTask> UserTask { get; set; }

        public DbSet<Project> Project { get; set; }
        public DbSet<Leaves> Leaves { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique()
            .HasDatabaseName("username");
            base.OnModelCreating(modelBuilder);

        /*  modelBuilder.Entity<Tasks>()
               .HasOne(t => t.project)
               .WithMany(p => p.tasks) */
        }

        public override int SaveChanges()
        {
            var userId = CurrentUserId ?? 0;
            // Ajoutez ici toute logique supplémentaire avant l'enregistrement, si nécessaire

            return base.SaveChanges();
        }


    }
}
