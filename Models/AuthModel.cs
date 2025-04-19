
using System.ComponentModel.DataAnnotations;

namespace AiTextSummarizeApi.Models
{
    public class AuthModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}