using System.ComponentModel.DataAnnotations;

namespace AiTextSummarizerApi.Models
{
    public class UserToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = null!;
    }
}