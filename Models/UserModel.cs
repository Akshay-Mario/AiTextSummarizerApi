
using System.ComponentModel.DataAnnotations;

namespace AiTextSummarizerApi.Models{
    public class User {

        [Key]
        public int id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]  
        public required string Username { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public ICollection<UserToken> Tokens { get; set; } = new List<UserToken>();
    }
}