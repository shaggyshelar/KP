using ESPL.KP.Entities.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ESPL.KP.Entities
{
    public class LibraryContext : IdentityDbContext<ESPLUser>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
           : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        public DbSet<AppModule> AppModules { get; set; }

    }
}
