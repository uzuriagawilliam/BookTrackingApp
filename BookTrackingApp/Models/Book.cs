using System.ComponentModel.DataAnnotations;

namespace BookTrackingApp.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        [Required(ErrorMessage =("Please enter a book title here"))]
        public string? Title { get; set; } /*= string.Empty;*/

        [Required(ErrorMessage = ("Please enter the book author here"))]
        public string? Author { get; set; } /*= string.Empty;*/

        [MaxLength(200)]
        [Required(ErrorMessage = ("Please add a short comment here"))]
        public string? Comment { get; set; }/* = string.Empty;*/

        [Required]
        public string? Status { get; set; }/* = string.Empty;*/

        [Required]
        public int Pages { get; set; }

        [Required(ErrorMessage = ("Please enter a number from 1 to 5 to rate this book"))]
        [Range(1, 5, ErrorMessage = ("Please enter a number from 1 to 5 to rate this book"))]
        public int Rating { get; set; }
        public string? FriendType { get; set; }
    }
}
