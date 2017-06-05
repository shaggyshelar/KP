using System;
using System.Linq;
using KP.Application.Interfaces;
using KP.Domain.Department;
using KP.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KP.Persistence
{
    public class ApplicationContext : IdentityDbContext<AppUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Department> Departments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {

            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelbuilder);
        }

        public void Save()
        {
            this.SaveChanges();
        }
    }

    public class DatabaseService : ApplicationContext, IDatabaseService
    {
        public DatabaseService(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
    }
}