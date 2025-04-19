
using System.ComponentModel.DataAnnotations;

namespace AiTextSummarizerApi.Models
{
    public class AuthModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}