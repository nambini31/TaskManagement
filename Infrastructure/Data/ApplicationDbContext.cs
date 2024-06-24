
using Microsoft.EntityFrameworkCore;
using Domain.Entity;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
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

        }
        public override int SaveChanges()
        {
            ExcludeProperty();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ExcludeProperty();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ExcludeProperty()
        {
            foreach (var entry in ChangeTracker.Entries<UserTask>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Property(e => e.isLeave).IsModified = false;
                }
            }
        }


    }
}
