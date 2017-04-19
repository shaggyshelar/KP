﻿using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace ESPL.KP.Entities
{
    public class LibraryContext : IdentityDbContext<ESPLUser>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        //public DbSet<CFGRolePermission> CFGRolePermission { get; set; }

        public DbSet<CFGUserDepartment> CFGUserDepartment { get; set; }

        public DbSet<MstArea> MstArea { get; set; }

        public DbSet<MstDepartment> MstDepartment { get; set; }

        public DbSet<MstDesignation> MstDesignation { get; set; }

        public DbSet<MstOccurrenceBook> MstOccurrenceBook { get; set; }

        public DbSet<MstOccurrenceStatus> MstOccurrenceStatus { get; set; }

        public DbSet<MstOccurrenceType> MstOccurrenceType { get; set; }

        public DbSet<MstPermission> MstPermission { get; set; }

        // public DbSet<MstRole> MstRole { get; set; }

        public DbSet<MstShift> MstShift { get; set; }

        // public DbSet<MstUser> MstUser { get; set; }

        // public DbSet<MstUserProfile> MstUserProfile { get; set; }

        public DbSet<OccurrenceAssignment> OccurrenceAssignment { get; set; }

        public DbSet<OccurrenceReviewHistory> OccurrenceReviewHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder){
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelbuilder);
        }

    }
}