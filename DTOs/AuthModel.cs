
using System.ComponentModel.DataAnnotations;

namespace AiTextSummarizerApi.DTOs
{
    public class AuthModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }

     public class UserInfo
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
    }

}
