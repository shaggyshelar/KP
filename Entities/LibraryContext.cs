using Microsoft.EntityFrameworkCore;

namespace Library.API.Entities {
    public class LibraryContext : DbContext {
        public LibraryContext (DbContextOptions<LibraryContext> options) : base (options) {
            Database.Migrate ();
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<CFGRolePermission> CFGRolePermission { get; set; }

        public DbSet<CFGUserDepartment> CFGUserDepartment { get; set; }

        public DbSet<MstArea> MstArea { get; set; }

        public DbSet<MstDepartment> MstDepartment { get; set; }

        public DbSet<MstDesignation> MstDesignation { get; set; }

        public DbSet<MstOccurrenceBook> MstOccurrenceBook { get; set; }

        public DbSet<MstOccurrenceStatus> MstOccurrenceStatus { get; set; }

        public DbSet<MstOccurrenceType> MstOccurrenceType { get; set; }

        public DbSet<MstPermission> MstPermission { get; set; }

        public DbSet<MstRole> MstRole { get; set; }

        public DbSet<MstShift> MstShift { get; set; }

        public DbSet<MstUser> MstUser { get; set; }

        public DbSet<MstUserProfile> MstUserProfile { get; set; }

        public DbSet<OccurrenceAssignment> OccurrenceAssignment { get; set; }

        public DbSet<OccurrenceReviewHistory> OccurrenceReviewHistory { get; set; }

        

    }
}
