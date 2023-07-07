using Microsoft.EntityFrameworkCore;

namespace BookTrackingApp.Models
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Friends> Friends { get; set; }
    }
}
