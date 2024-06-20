﻿
using Microsoft.EntityFrameworkCore;
using Domain.Entity;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UserTask> UserTask { get; set; }


    }
}
