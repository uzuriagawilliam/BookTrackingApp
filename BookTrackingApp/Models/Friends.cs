using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookTrackingApp.Models
{
    [PrimaryKey("Id")]
    public class Friends
    {
        public int Id { get; set; }
        [Required]
        public int FriendId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string FriendType { get; set; } = string.Empty;/*casual-close-lifetime*/
        [Required]
        public string? FriendName { get; set; } 
    }
}
