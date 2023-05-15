using Microsoft.EntityFrameworkCore;

namespace BookTrackingApp.Models
{
    public class BookRepository : IBookRepository
    {
        private readonly BookContext _context;
        public BookRepository(BookContext context)
        {
            _context = context;
        }
        public async Task AddBook(Book book)
        {
            var res = await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();            
        }

        public async Task<Book> GetBookById(int id)
        {
            //var book = await _context.Books.FindAsync(id);
            var book = await _context.Books.FirstOrDefaultAsync(B => B.BookId == id);
            if(book == null)
            {
                return null;
            }

            return book;
        }

        public Book GetBookByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Book>> GetBooks(string onerId, string status)
        {
            if(status == "" || status == null)
            {
                return await _context.Books.Where(b => b.OwnerId == onerId).ToListAsync();
            }
            else
            {
                return await _context.Books.Where(b => b.OwnerId == onerId).Where(s => s.Status == status).ToListAsync();
            }
        }

        public async Task RemoveBook(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }            
        }
    }
}
