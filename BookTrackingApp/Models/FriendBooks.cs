namespace BookTrackingApp.Models
{
    public class FriendBooks
    {
        public FriendBooks()
        {
            Books = new List<Book>();
        }
        public int Id { get; set; } 
        public Friends Friend { get; set; }
        public List<Book> Books { get; set; }
    }
}
