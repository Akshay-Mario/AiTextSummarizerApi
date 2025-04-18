
using System.ComponentModel.DataAnnotations;

namespace AiTextSummarizeApi.Models{
    public class AuthRequest {

        [Required]
        [EmailAddress]
        public required string Username { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }
    }
}