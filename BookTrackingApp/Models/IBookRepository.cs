namespace BookTrackingApp.Models
{
    public interface IBookRepository
    {
        public Task AddBook(Book book);
        public Task RemoveBook(int bookId);
        public Task<Book> GetBookById(int id);
        public Book GetBookByName(string name);
        public Task<IEnumerable<Book>> GetBooks(string onerId, string status);
        public Task<IEnumerable<Friends>> GetFriends(int id);
    }
}
