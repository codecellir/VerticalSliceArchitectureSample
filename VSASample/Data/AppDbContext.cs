using Microsoft.EntityFrameworkCore;
using VSASample.Entities;

namespace VSASample.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Book> Books => Set<Book>();
    }
}
